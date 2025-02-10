using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    public class Breed
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("origin")]
        public string Origin { get; set; }

        [JsonPropertyName("life_span")]
        public string LifeSpan { get; set; }

        [JsonPropertyName("health_issues")]
        public short HealthIssues { get; set; }

        [JsonPropertyName("intelligence")]
        public short Intelligence { get; set; }

        [JsonPropertyName("social_needs")]
        public short SocialNeeds { get; set; }

        [JsonPropertyName("wikipedia_url")]
        public string WikipediaUrl { get; set; }

        [JsonPropertyName("reference_image_id")]
        public string MainImageId { get; set; }

        [JsonIgnore]
        public ObservableCollection<string> ImagesIds { get; set; }
    }
}
