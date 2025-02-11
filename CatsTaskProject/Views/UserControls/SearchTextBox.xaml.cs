using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatsTaskProject.Views.UserControls
{
    /// <summary>
    /// Логика взаимодействия для SearchTextBox.xaml
    /// </summary>
    public partial class SearchTextBox : UserControl
    {
        public SearchTextBox()
        {
            InitializeComponent();
        }

        private string _placeholder;

        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                placeholderTextBlock.Text = _placeholder;
            }
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            inputTextBox.Clear();
            inputTextBox.Focus();
        }

        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputTextBox.Text))
                placeholderTextBlock.Visibility = Visibility.Visible;
            else placeholderTextBlock.Visibility = Visibility.Hidden;
        }

        private void inputTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            placeholderTextBlock.Visibility = Visibility.Hidden;
        }

        private void inputTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputTextBox.Text))
                placeholderTextBlock.Visibility = Visibility.Visible;
            else placeholderTextBlock.Visibility = Visibility.Hidden;
        }
    }
}
