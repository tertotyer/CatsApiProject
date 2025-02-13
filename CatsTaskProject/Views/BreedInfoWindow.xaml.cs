using CatsTaskProject.Models;
using CatsTaskProject.ViewModels;
using System.Windows;


namespace CatsTaskProject.Views
{
    public partial class BreedInfoWindow : WindowBase
    {
        public BreedInfoWindow(Breed passedBreed)
        {
            InitializeComponent();


            if (passedBreed is not null)
            {
                nameTextBlock.Text = passedBreed.Name;
                descriptionTextBlock.Text = passedBreed.Description is not null ? passedBreed.Description : descriptionTextBlock.Text;
                originTextBlock.Text = passedBreed.Origin is not null ? passedBreed.Origin : originTextBlock.Text;
                lifeSpanTextBlock.Text = passedBreed.LifeSpan is not null ? passedBreed.LifeSpan : lifeSpanTextBlock.Text;

                wikipediaHyperLink.NavigateUri = passedBreed.WikipediaUrl is not null ? new Uri(passedBreed.WikipediaUrl) : new Uri("www.wikipedia.org");

                for (int i = 0; i < passedBreed.HealthIssues; i++)
                    healthListBox.Items.Add(null);

                for (int i = 0; i < passedBreed.Intelligence; i++)
                    intelligenceListBox.Items.Add(null);

                for (int i = 0; i < passedBreed.SocialNeeds; i++)
                    socialNeedsListBox.Items.Add(null);
            }

            var viewModel = new BreedInfoViewModel(passedBreed);
            DataContext = viewModel;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
