using CatsTaskProject.Managers;
using CatsTaskProject.Managers.Interfaces;
using CatsTaskProject.Models;
using CatsTaskProject.ViewModels.Commands;
using CatsTaskProject.Views;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using static System.Net.Mime.MediaTypeNames;

namespace CatsTaskProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private const int BreedsLoadCountPerPage = 20;

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

        public ObservableCollection<ComboItem> BreedCountries
        {
            get => _breedCountries;
            set => this.RaiseAndSetIfChanged(ref _breedCountries, value);
        }

        public ICommand ExtendBreedCollectionCommand { get; }
        public ICommand FilterBreedsByOriginCommand { get; }
        public ICommand FilterBreedsByNameCommand { get; }
        public ICommand FilterBreedsByNameApiCommand { get; }
        public ICommand OpenBreedInfoWindowCommand { get; }


        internal void ResetPage()
        {
            _page = 0;
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

                FilteredBreeds = Breeds;
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
                        breeds.Remove(breeds[i]);
                        i--;
                        continue;
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
            }
        }


        private void FilterBreedsByOrigin(object selectedCountries)
        {
            IList<string> countries = (IList<string>)selectedCountries;
            if (countries is not null && countries.Count > 0)
            {
                FilteredBreeds = new ObservableCollection<Breed>(BreedManager.FilterBreedCollectionByOrigins(Breeds, countries));
            }
            else
            {
                FilteredBreeds = new ObservableCollection<Breed>(Breeds);
            }
        }

        private async Task FilterBreedsByName(object text)
        {
            if (text != null)
            {
                FilteredBreeds = new ObservableCollection<Breed>(BreedManager.FilterBreedCollectionByName(Breeds, text.ToString()));
                if (FilteredBreeds.Count < 1)
                {
                    await FilterBreedsByNameApi(text);
                    return;
                }
            }
            else
            {
                FilteredBreeds = new ObservableCollection<Breed>(Breeds);
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
                FilteredBreeds = new ObservableCollection<Breed>(BreedManager.FilterBreedCollectionByName(foundBreeds, text.ToString()));

                if (foundBreeds.Count > 0)
                    _isLastFilteredBreedsEmpty = false;
                else
                    _isLastFilteredBreedsEmpty = true;
            }
        }

        private void Breeds_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            BreedCountries = new ObservableCollection<ComboItem>(Breeds.Select(x => new ComboItem(x.Origin)).DistinctBy(x => x.Value).OrderBy(x => x.Value));
        }
    }
}
