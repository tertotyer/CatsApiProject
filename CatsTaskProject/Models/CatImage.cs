using CatsTaskProject.Converters;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    [JsonConverter(typeof(ImageJsonConverter))]
    public class CatImage
    {
        public required string Id { get; set; }

        public string Url { get; set; }

        public short Width { get; set; }

        public short Height { get; set; }

        public short BreedId { get; set; }
    }
}
