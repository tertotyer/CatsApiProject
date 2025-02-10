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
    public class ImageJsonConverter : JsonConverter<CatImage>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(CatImage).IsAssignableFrom(typeToConvert);
        }

        public override CatImage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using JsonDocument jsonDoc = JsonDocument.ParseValue(ref reader);
            CatImage image = new()
            {
                Id = jsonDoc.RootElement.TryGetProperty("id", out JsonElement valId) ? valId.GetString() : null,
                Url = jsonDoc.RootElement.TryGetProperty("url", out JsonElement valUrl) ? valUrl.GetString() : null,
                Width = jsonDoc.RootElement.TryGetProperty("width", out JsonElement valWidth) ? valWidth.GetInt16() : (short)-1,
                Height = jsonDoc.RootElement.TryGetProperty("width", out JsonElement valHeight) ? valHeight.GetInt16() : (short)-1,
            };
            return image;
        }

        public override void Write(Utf8JsonWriter writer, CatImage value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
