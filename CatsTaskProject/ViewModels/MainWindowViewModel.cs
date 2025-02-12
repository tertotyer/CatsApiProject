using CatsTaskProject.Managers;
using CatsTaskProject.Models;
using CatsTaskProject.ViewModels.Commands;
using DynamicData;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace CatsTaskProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _page = 0;
        private ObservableCollection<Breed> _filteredBreeds;

        public MainWindowViewModel()
        {
            FilterBreedsByNameCommand = new DelegateCommand(text => FilterBreedsByName(text));

            Breeds = new ObservableCollection<Breed>();
            GetCatBreeds();
            FilteredBreeds = Breeds;

            SearchBreedsCommands = new DelegateCommand(_ => SearchBreed());
        }

        public ObservableCollection<Breed> Breeds { get; set; }
        public ObservableCollection<Breed> FilteredBreeds
        {
            get => _filteredBreeds;
            set => this.RaiseAndSetIfChanged(ref _filteredBreeds, value);
        }

        public ICommand FilterBreedsByNameCommand { get; }
        public ICommand SearchBreedsCommands { get; }

        internal void ResetPage()
        {
            _page = 0;
        }

        private async void GetCatBreeds()
        {
            CatAPIManager apiManager = CatAPIManager.Instance;
            string jsonResult = await apiManager.GetBreeds(20, _page++);

            IList<Breed> breeds = JsonSerializer.Deserialize<IList<Breed>>(jsonResult);
            if (breeds.Count > 0)
            {
                Breeds.AddRange(breeds);
                LoadBreedsImages(breeds);
            }
        }

        private async void LoadBreedsImages(IList<Breed> breeds)
        {
            ImageManager imageManager = new();
            for (int i = 0; i < breeds.Count; i++)
            {
                CatAPIManager apiManager = CatAPIManager.Instance;
                if (breeds[i].MainImageId is null)
                {
                    string jsonImage = await apiManager.GetBreedImages(breeds[i].Id, 1);

                    List<CatImage> images = JsonSerializer.Deserialize<List<CatImage>>(jsonImage);
                    if (images.Count > 0)
                    {
                        breeds[i].MainImageId = images[0].Id;
                        await imageManager.LoadImage(images[0].Url);
                    }
                    else
                    {
                        breeds.Remove(breeds[i]);
                        i--;
                        continue;
                    }
                }
                else if (!imageManager.ImageAlreadyLoadedById(breeds[i].MainImageId))
                {
                    string jsonImage = await apiManager.GetImageById(breeds[i].MainImageId);

                    await imageManager.LoadImage(JsonSerializer.Deserialize<CatImage>(jsonImage).Url);
                }
            }
        }

        private void FilterBreedsByName(object text)
        {
            FilteredBreeds = new ObservableCollection<Breed>(BreedManager.FilterBreedCollectionByName(Breeds, text.ToString()));
        }

        private void SearchBreed()
        {

        }
    }
}
