using CatsTaskProject.ViewModels;
using CatsTaskProject.UserControls;
using System.Windows;
using System.Windows.Controls;
using CatsTaskProject.Models;

namespace CatsTaskProject.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : WindowBase
    {
        private const int PageCountBeforeFetchElements = 2;
        private double _previousExtentHeight;

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
                viewModel.FilterBreedsByNameApiCommand.Execute(textBox.Text);
            }
        }

        private void breedsListBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (e.VerticalChange > 0 && _previousExtentHeight != e.ExtentHeight &&
                string.IsNullOrEmpty(searchTextBox.Text) &&
                e.ExtentHeight - e.VerticalOffset - e.ViewportHeight * PageCountBeforeFetchElements < 0) 
            {
                MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
                viewModel.ExtendBreedCollectionCommand.Execute(null);

                _previousExtentHeight = e.ExtentHeight;
            }
        }

        private void CheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            searchTextBox.IsEnabled = false;
            searchTextBox.Text = string.Empty;

            List<string> selectedCountries = new();
            for (int i = 0; i < breedCountriesComboBox.Items.Count; i++)
            {
                ComboItem item = breedCountriesComboBox.Items[i] as ComboItem;
                if (item.Ischecked)
                    selectedCountries.Add(item.Value);
            }

            if (selectedCountries.Count < 1)
            {
                searchTextBox.IsEnabled = true;
                breedCountriesComboBox.Text = "Select countries";
            }
            else
                breedCountriesComboBox.Text = selectedCountries.Count.ToString();

            MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
            viewModel.FilterBreedsByOriginCommand.Execute(selectedCountries);
        }

        private void breedCountriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            breedCountriesComboBox.SelectedIndex = -1;
        }

        private void favoriteButton_Click(object sender, RoutedEventArgs e)
        {
            searchTextBox.Text = string.Empty;

            foreach (ComboItem item in breedCountriesComboBox.Items)
            {
                item.Ischecked = false;
            }
        }
    }
}