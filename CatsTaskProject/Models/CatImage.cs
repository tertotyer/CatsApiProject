using CatsTaskProject.Converters;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    [JsonConverter(typeof(ImageJsonConverter))]
    public class CatImage
    {
        public required string Id { get; set; }
        public string Url { get; set; }
        public ushort Width { get; set; }
        public ushort Height { get; set; }

        public string BreedId { get; set; }
    }
}
