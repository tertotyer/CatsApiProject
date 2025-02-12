using CatsTaskProject.ViewModels;
using CatsTaskProject.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace CatsTaskProject.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void searchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            SearchTextBox textBox = sender as SearchTextBox;
            MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
            viewModel.FilterBreedsByNameCommand.Execute(textBox.Text);
        }
    }
}