using CatsTaskProject.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Converters
{
    public class BreedJsonConverter : JsonConverter<Breed>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Breed).IsAssignableFrom(typeToConvert);
        }

        public override Breed Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader);
            Breed breed = new()
            {
                Id = jsonDoc.RootElement.GetProperty("id").GetString(),
                Name = jsonDoc.RootElement.GetProperty("name").GetString(),
                Description = jsonDoc.RootElement.GetProperty("description").GetString(),
                Origin = jsonDoc.RootElement.GetProperty("origin").GetString(),
                LifeSpan = jsonDoc.RootElement.GetProperty("life_span").GetString(),
                HealthIssues = jsonDoc.RootElement.GetProperty("health_issues").GetInt16(),
                Intelligence = jsonDoc.RootElement.GetProperty("intelligence").GetInt16(),
                SocialNeeds = jsonDoc.RootElement.GetProperty("social_needs").GetInt16(),
                WikipediaUrl = jsonDoc.RootElement.GetProperty("wikipedia_url").GetString(),
                MainImageId = jsonDoc.RootElement.GetProperty("reference_image_id").GetString(),
            };
            return breed;
        }

        public override void Write(Utf8JsonWriter writer, Breed value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
