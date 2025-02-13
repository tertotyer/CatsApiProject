using CatsTaskProject.Converters;
using CatsTaskProject.Managers;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    [JsonConverter(typeof(BreedJsonConverter))]
    public class Breed : ReactiveObject
    {
        public required string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Origin { get; set; }

        public string LifeSpan { get; set; }

        public short HealthIssues { get; set; }

        public short Intelligence { get; set; }

        public short SocialNeeds { get; set; }

        public string WikipediaUrl { get; set; }

        
        public string MainImageId { get; set; }

        private CatImage _mainImage;
        public CatImage MainImage
        {
            get => _mainImage;
            set => this.RaiseAndSetIfChanged(ref _mainImage, value);
        }


        public ObservableCollection<string> ImagesIds { get; set; }

        public bool IsFavorite { get; set; }


        public void AddImage(CatImage image)
        {
            MainImage = image;
            MainImageId = image.Id;
            MainImage.LocalImagePath = new ImageManager().GetImagePath(MainImage.Url);
        }

        public void AddImage(string imagePath)
        {
            MainImage = new CatImage()
            {
                Id = MainImageId,
            };

            MainImage.LocalImagePath = imagePath;
        }
    }
}
