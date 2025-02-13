using CatsTaskProject.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CatsTaskProject.ViewModels
{
    public class BreedInfoViewModel : ViewModelBase
    {
        public BreedInfoViewModel() : base()
        {
            OpenWikipediaCommand = new DelegateCommand(urlObj =>
            {
                string url = urlObj.ToString();
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
            });
        }

        public ICommand OpenWikipediaCommand { get; }
    }
}
