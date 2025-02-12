using CatsTaskProject.Converters;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    [JsonConverter(typeof(BreedJsonConverter))]
    public class Breed
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

        public ObservableCollection<string> ImagesIds { get; set; }

        public bool IsFavorite { get; set; }
    }
}
