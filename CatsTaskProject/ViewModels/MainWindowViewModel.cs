using CatsTaskProject.Managers;
using CatsTaskProject.Models;
using DynamicData.Binding;
using System.Text.Json;
using System.Windows.Input;

namespace CatsTaskProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _page = 0;

        public MainWindowViewModel()
        {
            GetCatBreedsCommand = new DelegateCommand(GetCatBreeds);
        }

        public ICommand GetCatBreedsCommand { get; }

        internal void ResetPage()
        {
            _page = 0;
        }

        private async void GetCatBreeds()
        {
            CatAPIManager apiManager = CatAPIManager.Instance;
            string jsonResult = await apiManager.GetBreeds(20, _page++);

            ICollection<Breed> breeds = JsonSerializer.Deserialize<ICollection<Breed>>(jsonResult);
            LoadBreedsImages(breeds);
        }

        private async void LoadBreedsImages(ICollection<Breed> breeds)
        {
            ImageManager imageManager = new();
            foreach (Breed breed in breeds)
            {
                CatAPIManager apiManager = CatAPIManager.Instance;
                if (breed.MainImageId is null)
                {
                    string jsonImage = await apiManager.GetBreedImages(breed.Id, 1);

                    List<CatImage> images = JsonSerializer.Deserialize<List<CatImage>>(jsonImage);
                    breed.MainImageId = images[0].Id;
                    await imageManager.LoadImage(images[0].Url);
                }
                else if (!imageManager.ImageAlreadyLoadedById(breed.MainImageId))
                {
                    var jsonImage = await apiManager.GetImageById(breed.MainImageId);

                    await imageManager.LoadImage(JsonSerializer.Deserialize<CatImage>(jsonImage).Url);
                }
            }
        }
    }
}
