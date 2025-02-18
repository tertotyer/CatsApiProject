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
                SocialNeeds = jsonDoc.RootElement.TryGetProperty("social_needs", out JsonElement valSocial) ? valSocial.GetInt16() : (short)-1,
                WikipediaUrl = jsonDoc.RootElement.TryGetProperty("wikipedia_url", out JsonElement valWikipedia) ? valWikipedia.GetString() : null,
                MainImageId = jsonDoc.RootElement.TryGetProperty("reference_image_id", out JsonElement valImageId) ? valImageId.GetString() : null,
                MainImage = jsonDoc.RootElement.TryGetProperty("image", out JsonElement valImage) ? JsonSerializer.Deserialize<CatImage>(valImage) : null,
            };
            return breed;
        }

        public override void Write(Utf8JsonWriter writer, Breed breed, JsonSerializerOptions options)
        {

            writer.WriteStartObject();

            writer.WriteString("id", breed.Id);
            writer.WriteString("name", breed.Name);
            writer.WriteString("description", breed.Description);
            writer.WriteString("origin", breed.Origin);
            writer.WriteString("life_span", breed.LifeSpan);
            writer.WriteNumber("health_issues", breed.HealthIssues);
            writer.WriteNumber("intelligence", breed.Intelligence);
            writer.WriteNumber("social_needs", breed.SocialNeeds);
            writer.WriteString("wikipedia_url", breed.WikipediaUrl);
            writer.WriteString("reference_image_id", breed.MainImageId);

            writer.WriteStartObject("image");
            writer.WriteString("id", breed.MainImage.Id);
            writer.WriteNumber("width", breed.MainImage.Width);
            writer.WriteNumber("height", breed.MainImage.Height);
            writer.WriteString("url", breed.MainImage.Url);
            writer.WriteEndObject();

            writer.WriteEndObject();
        }
    }
}
