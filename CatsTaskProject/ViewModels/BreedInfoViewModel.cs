using CatsTaskProject.Managers;
using CatsTaskProject.Models;
using CatsTaskProject.ViewModels.Commands;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace CatsTaskProject.ViewModels
{
    public class BreedInfoViewModel : ViewModelBase
    {
        private const int BreedLoadImagesCount = 8;
        private const int ChangeImageTimerIntervalSeconds = 5;

        private DispatcherTimer _changeImageTimer;
        private Breed _currentBreed;
        private int _currentImageIndex;

        public BreedInfoViewModel(Breed currentBreed) : base()
        {
            _currentBreed = currentBreed;
            OpenWikipediaCommand = new DelegateCommand(urlObj =>
            {
                string url = urlObj.ToString();
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            });

            LoadBreedImagesAsyncCommand = new DelegateCommand(async _ =>
            {
                _currentBreed.Images = new ObservableCollection<CatImage>(await new ImageManager().GetBreedImages(_currentBreed.Id, BreedLoadImagesCount));
                if (_currentBreed.Images.Count > 0)
                {
                    foreach (var image in _currentBreed.Images)
                        image.PropertyChanged += BreedInfoViewModel_PropertyChanged;

                    LoadBreedImages(_currentBreed.Images);
                }
                else
                {
                    CatImage noneCatImage = new CatImage()
                    {
                        Id = "NoneCat",
                        Url = "NoneCat.jpeg",
                        LocalImagePath = new ImageManager().GetImagePath("NoneCat.jpeg")
                    };
                    _currentBreed.Images.Add(noneCatImage);
                }
            });

            NextImageCommand = new DelegateCommand(_ =>
            {
                if (_currentBreed.Images is not null && _currentBreed.Images.Count > 1)
                {
                    _currentImageIndex = ++_currentImageIndex < _currentBreed.Images.Count ? _currentImageIndex : 0;
                    CurrentImageLocalPath = _currentBreed.Images[_currentImageIndex].LocalImagePath;

                    _changeImageTimer.Stop();
                    _changeImageTimer.Start();
                }
            });

            PreviousImageCommand = new DelegateCommand(_ =>
            {
                if (_currentBreed.Images is not null && _currentBreed.Images.Count > 1)
                {
                    _currentImageIndex = --_currentImageIndex < 0 ? _currentBreed.Images.Count - 1 : _currentImageIndex;
                    CurrentImageLocalPath = _currentBreed.Images[_currentImageIndex].LocalImagePath;

                    _changeImageTimer.Stop();
                    _changeImageTimer.Start();
                }
            });

            _changeImageTimer = new();
            _changeImageTimer.Interval = new TimeSpan(0, 0, ChangeImageTimerIntervalSeconds);
            _changeImageTimer.Tick += changeTimer_Tick;
            _changeImageTimer.Start();
        }

        private void changeTimer_Tick(object sender, EventArgs e)
        {
            _currentImageIndex = ++_currentImageIndex < _currentBreed.Images.Count ? _currentImageIndex : 0;
            CurrentImageLocalPath = _currentBreed.Images[_currentImageIndex].LocalImagePath;
        }

        private void BreedInfoViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            CatImage changedImage = sender as CatImage;
            if (e.PropertyName == "LocalImagePath" && changedImage.Id == _currentBreed.Images?[_currentImageIndex].Id)
            {
                CurrentImageLocalPath = _currentBreed.Images[_currentImageIndex].LocalImagePath;
            }
        }

        public ICommand OpenWikipediaCommand { get; }
        public ICommand LoadBreedImagesAsyncCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand PreviousImageCommand { get; }

        public Breed CurrentBreed { get { return _currentBreed; } }

        public ObservableCollection<bool> HealthAssessment { get; private set; }
        public ObservableCollection<bool> IntelligenceAssessment { get; private set; }
        public ObservableCollection<bool> SocialNeedsAssessment { get; private set; }

        private string _currentImageLocalPath;
        public string CurrentImageLocalPath
        {
            get => _currentImageLocalPath;
            set => this.RaiseAndSetIfChanged(ref _currentImageLocalPath, value);
        }

        private async void LoadBreedImages(ICollection<CatImage> images)
        {
            ImageManager imageManager = new();
            foreach (CatImage image in images)
            {
                string fullPath = string.Empty;
                bool result = imageManager.ImageAlreadyLoadedById(image.Id);
                if (!result)
                {
                    await imageManager.LoadImage(image.Url);
                }
                image.LocalImagePath = imageManager.GetImagePath(image.Url);
            }
        }
    }
}
