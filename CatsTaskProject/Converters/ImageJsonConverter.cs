using CatsTaskProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
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
                Id = jsonDoc.RootElement.GetProperty("id").GetString(),
                Url = jsonDoc.RootElement.GetProperty("url").GetString(),
                Width = jsonDoc.RootElement.GetProperty("width").GetUInt16(),
                Height = jsonDoc.RootElement.GetProperty("height").GetUInt16(),
            };
            return image;
        }

        public override void Write(Utf8JsonWriter writer, CatImage value, JsonSerializerOptions options)
        {
            JsonSerializer.Serialize(writer, value, options);
        }
    }
}
