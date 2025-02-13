﻿using CatsTaskProject.Managers;
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
        private int _page = 0;
        private bool _isLastFilteredBreedsEmpty = false;
        private ObservableCollection<Breed> _filteredBreeds;

        public MainWindowViewModel()
        {
            FilterBreedsByNameCommand = new DelegateCommand(async text => await FilterBreedsByName(text));
            FilterBreedsByNameApiCommand = new DelegateCommand(async text => await FilterBreedsByNameApi(text));
            OpenBreedInfoWindowCommand = new DelegateCommand(obj =>
            {
                BreedInfoWindow breedInfoWindow = new BreedInfoWindow();
                breedInfoWindow.ShowDialog();
            });
            Breeds = new ObservableCollection<Breed>();
            FilteredBreeds = new ObservableCollection<Breed>();

            GetCatBreeds();
        }

        public ObservableCollection<Breed> Breeds { get; set; }
        public ObservableCollection<Breed> FilteredBreeds
        {
            get => _filteredBreeds;
            set => this.RaiseAndSetIfChanged(ref _filteredBreeds, value);
        }

        public ICommand FilterBreedsByNameCommand { get; }
        public ICommand FilterBreedsByNameApiCommand { get; }
        public ICommand OpenBreedInfoWindowCommand { get; }

        internal void ResetPage()
        {
            _page = 0;
        }

        private async void GetCatBreeds()
        {
            BreedManager breedManager = new();
            IList<Breed> loadedBreeds = await breedManager.GetBreeds(20, _page++);
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
                        breeds[i].AddImage(image);
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
                    string fullPath = string.Empty;
                    bool result = imageManager.ImageAlreadyLoadedById(breeds[i].MainImageId, out fullPath);
                    if (!result)
                    {
                        CatImage image = await imageManager.GetImageById(breeds[i].MainImageId);
                        await imageManager.LoadImage(image.Url);
                        breeds[i].AddImage(image);
                    }
                    else
                    {
                        breeds[i].AddImage(fullPath);
                    }
                }
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
                {
                    _isLastFilteredBreedsEmpty = false;
                }
                else
                {
                    _isLastFilteredBreedsEmpty = true;
                }
            }
        }


    }
}
