using CatsTaskProject.Managers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
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

        private async void GetCatBreeds()
        {
            var apiManager = CatAPIManager.Instance;
            var result = await apiManager.GetBreeds(20, _page++);
        }
    }
}
