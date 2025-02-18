using CatsTaskProject.Managers;
using CatsTaskProject.Managers.Interfaces;
using CatsTaskProject.Models;
using CatsTaskProject.ViewModels.Commands;
using CatsTaskProject.Views;
using DynamicData;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Runtime.Caching;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatsTaskProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const int BreedsLoadCountPerPage = 20;
        private const string FavoritesFileName = "favorites.json";

        private int _page = 0;
        private bool _isLastFilteredBreedsEmpty = false;

        private ObservableCollection<Breed> _breeds;
        private ObservableCollection<Breed> _filteredBreeds;
        private ObservableCollection<ComboItem> _breedCountries;

        public MainWindowViewModel()
        {
            Breeds = new ObservableCollection<Breed>();
            Breeds.CollectionChanged += Breeds_CollectionChanged;

            FilteredBreeds = new ObservableCollection<Breed>();

            LoadFirstBreedCollectionCommand = new DelegateCommand(async _ => await LoadFirstBreedCollection());
            ExtendBreedCollectionCommand = new DelegateCommand(async _ => await GetCatBreeds());
            FilterBreedsByOriginCommand = new DelegateCommand(obj => FilterBreedsByOrigin(obj));
            FilterBreedsByNameCommand = new DelegateCommand(async text => await FilterBreedsByName(text));
            FilterBreedsByNameApiCommand = new DelegateCommand(async text => await FilterBreedsByNameApi(text));
            OpenBreedInfoWindowCommand = new DelegateCommand(obj =>
            {
                Breed breed = obj as Breed;
                BreedInfoWindow breedInfoWindow = new(breed);
                breedInfoWindow.ShowDialog();
            });

            FavoriteCommand = new DelegateCommand(obj =>
            {
                Breed breed = obj as Breed;
                if (breed is not null)
                {
                    breed.IsFavorite = !breed.IsFavorite;

                    File.Create(FavoritesFileName).Close();
                    File.AppendAllText(FavoritesFileName, JsonSerializer.Serialize(Breeds.Where(x => x.IsFavorite)));

                    FilteredBreeds = new(Breeds.OrderBy(x => x.IsFavorite ? 0 : 1).ThenBy(x => x.Name));
                }
            });
        }

        public ObservableCollection<Breed> Breeds
        {
            get => _breeds;
            set => this.RaiseAndSetIfChanged(ref _breeds, value);
        }

        public ObservableCollection<Breed> FilteredBreeds
        {
            get => _filteredBreeds;
            set => this.RaiseAndSetIfChanged(ref _filteredBreeds, value);
        }

        public ObservableCollection<ComboItem> BreedsOrigins
        {
            get => _breedCountries;
            set => this.RaiseAndSetIfChanged(ref _breedCountries, value);
        }

        public ICommand LoadFirstBreedCollectionCommand { get; }
        public ICommand ExtendBreedCollectionCommand { get; }
        public ICommand FilterBreedsByOriginCommand { get; }
        public ICommand FilterBreedsByNameCommand { get; }
        public ICommand FilterBreedsByNameApiCommand { get; }
        public ICommand OpenBreedInfoWindowCommand { get; }
        public ICommand FavoriteCommand { get; }


        internal void ResetPage()
        {
            _page = 0;
        }

        private async Task LoadFirstBreedCollection()
        {
            await GetCatBreeds();
            AddFavoriteBreeds();
        }

        private void AddFavoriteBreeds()
        {
            IList<Breed> favoriteBreeds = new List<Breed>();
            if (File.Exists(FavoritesFileName))
            {
                string fileText = File.ReadAllText(FavoritesFileName);
                if (!string.IsNullOrEmpty(fileText))
                {
                    favoriteBreeds = JsonSerializer.Deserialize<IList<Breed>>(fileText);

                    if (favoriteBreeds.Count > 0)
                    {
                        foreach (Breed breed in favoriteBreeds)
                        {
                            Breed item = Breeds.FirstOrDefault(x => x.Id == breed.Id);
                            if (item is null)
                            {
                                breed.MainImage.LocalImagePath = new ImageManager().GetImagePath(breed.MainImage.Url);
                                breed.IsFavorite = true;
                                Breeds.Add(breed);

                                MemoryCache.Default.Add(breed.Id, breed, new CacheItemPolicy()
                                {
                                    AbsoluteExpiration = DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                                });
                            }
                            else
                                item.IsFavorite = true;
                        }

                        FilteredBreeds = new(Breeds.OrderBy(x => x.IsFavorite ? 0 : 1).ThenBy(x => x.Name));
                    }
                }
            }
        }

        private async Task GetCatBreeds()
        {
            BreedManager breedManager = new();
            IList<Breed> loadedBreeds = await breedManager.GetBreeds(BreedsLoadCountPerPage, _page++);
            if (loadedBreeds.Count > 0)
            {
                IList<Breed> newBreeds = BreedManager.GetAllNewBreeds(Breeds, loadedBreeds);
                Breeds.AddRange(newBreeds);
                LoadBreedsImages(newBreeds);

                FilteredBreeds = new(Breeds.OrderBy(x => x.IsFavorite ? 0 : 1).ThenBy(x => x.Name));
            }
        }

        private async void LoadBreedsImages(IList<Breed> breeds)
        {
            ImageManager imageManager = new();
            for (int i = 0; i < breeds.Count; i++)
            {
                if (breeds[i].MainImageId is null)
                {
                    IList<CatImage> images = await imageManager.GetBreedImages(breeds[i].Id, 1);
                    if (images.Count > 0)
                    {
                        CatImage image = images[0];
                        await imageManager.LoadImage(image.Url);
                        breeds[i].SetMainImage(image);
                    }
                    else
                    {
                        CatImage noneCatImage = new CatImage()
                        {
                            Id = "NoneCat",
                            Url = "NoneCat.jpeg",
                        };
                        breeds[i].SetMainImage(noneCatImage);
                    }
                }
                else
                {
                    bool result = imageManager.ImageAlreadyLoadedById(breeds[i].MainImageId);
                    if (!result)
                    {
                        await imageManager.LoadImage(breeds[i].MainImage.Url);
                    }
                }
                breeds[i].MainImage.LocalImagePath = imageManager.GetImagePath(breeds[i].MainImage.Url);
                MemoryCache.Default.Add(breeds[i].Id, breeds[i], new System.Runtime.Caching.CacheItemPolicy()
                {
                    AbsoluteExpiration = DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                });
            }
        }

        private void FilterBreedsByOrigin(object selectedCountries)
        {
            IList<string> countries = (IList<string>)selectedCountries;
            if (countries is not null && countries.Count > 0)
            {
                FilteredBreeds = new(BreedManager.FilterBreedsByOrigins(Breeds, countries));
            }
            else
            {
                FilteredBreeds = new(Breeds.OrderBy(x => x.IsFavorite ? 0 : 1).ThenBy(x => x.Name));
            }
        }

        private async Task FilterBreedsByName(object text)
        {
            if (!string.IsNullOrEmpty(text.ToString()))
            {
                FilteredBreeds = new(BreedManager.FilterBreedsByName(Breeds, text.ToString()));
                if (FilteredBreeds.Count < 1)
                {
                    await FilterBreedsByNameApi(text);
                    return;
                }
            }
            else
            {
                FilteredBreeds = new(Breeds.OrderBy(x => x.IsFavorite ? 0 : 1).ThenBy(x => x.Name));
            }
            _isLastFilteredBreedsEmpty = false;
        }

        private async Task FilterBreedsByNameApi(object text)
        {
            if (!string.IsNullOrEmpty(text.ToString()) && !_isLastFilteredBreedsEmpty)
            {
                BreedManager breedManager = new();
                IList<Breed> foundBreeds = await breedManager.SearchBreedsByName(text.ToString());

                IList<Breed> newBreeds = BreedManager.GetAllNewBreeds(Breeds, foundBreeds);

                Breeds.AddRange(newBreeds);
                LoadBreedsImages(newBreeds);

                foreach (var breed in foundBreeds)
                {
                    breed.MainImage = Breeds.FirstOrDefault(x => x.Id == breed.Id)?.MainImage;
                }
                FilteredBreeds = new(BreedManager.FilterBreedsByName(foundBreeds, text.ToString()));

                if (foundBreeds.Count > 0)
                    _isLastFilteredBreedsEmpty = false;
                else
                    _isLastFilteredBreedsEmpty = true;
            }
        }

        private void Breeds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BreedsOrigins = new(Breeds.Select(x => new ComboItem(x.Origin)).DistinctBy(x => x.Value).OrderBy(x => x.Value));
        }
    }
}
