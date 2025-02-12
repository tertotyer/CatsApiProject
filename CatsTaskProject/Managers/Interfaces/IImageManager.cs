using CatsTaskProject.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatsTaskProject.Managers.Interfaces
{
    interface IImageManager
    {
        public string ImagePath { get; set; }

        public Task<CatImage> GetImageById(string imageId);
        public Task<IList<CatImage>> GetBreedImages(string breedId, int quantity);

        public Task LoadImage(string imageUrl);

        public bool ImageAlreadyLoadedById(string imageId);
        public bool ImageAlreadyLoadedByUrl(string imageUrl);
    }
}
