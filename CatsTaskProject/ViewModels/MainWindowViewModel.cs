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
            string jsonResult = await apiManager.GetBreeds(20, 2);

            IList<Breed> breeds = JsonSerializer.Deserialize<IList<Breed>>(jsonResult);
            LoadBreedsImages(breeds);
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
                    var jsonImage = await apiManager.GetImageById(breeds[i].MainImageId);

                    await imageManager.LoadImage(JsonSerializer.Deserialize<CatImage>(jsonImage).Url);
                }
            }
        }
    }
}
