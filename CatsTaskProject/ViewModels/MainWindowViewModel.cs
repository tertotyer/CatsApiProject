using CatsTaskProject.Managers;
using CatsTaskProject.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatsTaskProject.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private int _page = 0;

        public MainWindowViewModel()
        {
            GetCatBreedsCommand = new DelegateCommand(GetImageById);
        }

        public ICommand GetCatBreedsCommand { get; }

        internal void ResetPage()
        {
            _page = 0;
        }

        private async void GetCatBreeds()
        {
            var apiManager = CatAPIManager.Instance;
            var result = await apiManager.GetBreeds(20, _page++);
        }

        private async void GetImageById()
        {
            var apiManager = CatAPIManager.Instance;
            var result = await apiManager.GetImageById("0XYvRd7oD");

            CatImage image = JsonSerializer.Deserialize<CatImage>(result);
        }
    }
}
