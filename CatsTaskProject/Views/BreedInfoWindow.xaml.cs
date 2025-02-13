using CatsTaskProject.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                originTextBlock.Text = passedBreed.Origin is not null ? string.Concat("Origin: ", passedBreed.Origin) : originTextBlock.Text;
                lifeSpanTextBlock.Text = passedBreed.LifeSpan is not null ? string.Concat("Life span: ", passedBreed.LifeSpan) : lifeSpanTextBlock.Text;
                
                //TODO: Add more values

                wikipediaHyperLink.NavigateUri = passedBreed.WikipediaUrl is not null ? new Uri(passedBreed.WikipediaUrl) : new Uri("www.wikipedia.org");
            }
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
