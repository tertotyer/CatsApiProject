using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CatsTaskProject.UserControls
{

    public partial class SearchTextBox : UserControl
    {
        public static readonly DependencyProperty SearchCommandProperty = 
            DependencyProperty.Register("SearchCommand", typeof(ICommand), typeof(SearchTextBox), new PropertyMetadata(default(ICommand)));

        public ICommand SearchCommand
        {
            get { return (ICommand)GetValue(SearchCommandProperty); }
            set { SetValue(SearchCommandProperty, value); }
        }

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

        public string Text
        {
            get => inputTextBox.Text;
            set => inputTextBox.Text = value;
        }

        private void inputTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(inputTextBox.Text) && !inputTextBox.IsFocused)
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

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchCommand.Execute(inputTextBox.Text);
        }
    }
}
