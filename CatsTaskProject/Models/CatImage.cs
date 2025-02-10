using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    public class CatImage
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("width")]
        public ushort Width { get; set; }

        [JsonPropertyName("height")]
        public ushort Height { get; set; }

        [JsonIgnore]
        public string BreedId { get; set; }
    }
}
