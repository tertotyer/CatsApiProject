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
            if ((e.Key >= System.Windows.Input.Key.A && e.Key <= System.Windows.Input.Key.Z) ||
                e.Key == System.Windows.Input.Key.Back)
            {
                SearchTextBox textBox = sender as SearchTextBox;
                MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
                viewModel.FilterBreedsByNameCommand.Execute(textBox.Text);
            }
            else if (e.Key == System.Windows.Input.Key.Enter)
            {
                SearchTextBox textBox = sender as SearchTextBox;
                MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
                viewModel.SearchBreedsByNameApiCommand.Execute(textBox.Text);
            }
        }
    }
}