using CatsTaskProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

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
                Id = jsonDoc.RootElement.TryGetProperty("id", out JsonElement valId) ? valId.GetString() : null,
                Name = jsonDoc.RootElement.TryGetProperty("name", out JsonElement valName) ? valName.GetString() : null,
                Description = jsonDoc.RootElement.TryGetProperty("description", out JsonElement valDescription) ? valDescription.GetString() : null,
                Origin = jsonDoc.RootElement.TryGetProperty("origin", out JsonElement valOrigin) ? valOrigin.GetString() : null,
                LifeSpan = jsonDoc.RootElement.TryGetProperty("life_span", out JsonElement valLife) ? valLife.GetString() : null,
                HealthIssues = jsonDoc.RootElement.TryGetProperty("health_issues", out JsonElement valHealth) ? valHealth.GetInt16() : (short)-1,
                Intelligence = jsonDoc.RootElement.TryGetProperty("intelligence", out JsonElement valIntelligence) ? valIntelligence.GetInt16() : (short)-1,
                SocialNeeds =   jsonDoc.RootElement.TryGetProperty("social_needs", out JsonElement valSocial) ? valSocial.GetInt16() : (short)-1,
                WikipediaUrl = jsonDoc.RootElement.TryGetProperty("wikipedia_url", out JsonElement valWikipedia) ? valWikipedia.GetString() : null,
                MainImageId = jsonDoc.RootElement.TryGetProperty("reference_image_id", out JsonElement valImage) ? valImage.GetString() : null,
            };
            return breed;
        }

        public override void Write(Utf8JsonWriter writer, Breed value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
