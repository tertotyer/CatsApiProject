using CatsTaskProject.Converters;
using CatsTaskProject.Managers;
using ReactiveUI;
using System.Text.Json.Serialization;

namespace CatsTaskProject.Models
{
    [JsonConverter(typeof(ImageJsonConverter))]
    public class CatImage : ReactiveObject
    {
        public required string Id { get; set; }

        public string Url { get; set; }

        public short Width { get; set; }

        public short Height { get; set; }

        public short BreedId { get; set; }

        private string _localImagePath;
        public string LocalImagePath
        {
            get => _localImagePath;
            set => this.RaiseAndSetIfChanged(ref _localImagePath, value);
        }
    }
}
