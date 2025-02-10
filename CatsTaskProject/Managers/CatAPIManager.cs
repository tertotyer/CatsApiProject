using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CatsTaskProject.Managers
{
    internal class CatAPIManager
    {
        public static readonly CatAPIManager Instance = new();

        private readonly HttpClient httpClient = new();
        
        private CatAPIManager()
        {
        }

        public async Task<string> GetImageById(string imageId)
        {
            string url = $"https://api.thecatapi.com/v1/images/{imageId}";
            return await httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreeds(int quantity, int page)
        {
            string url = $"http://api.thecatapi.com/v1/breeds?limit={quantity}&page={page}";
            return await httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreedById(string id)
        {
            string url = $"http://api.thecatapi.com/v1/breeds/{id}";
            return await httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreedImages(string breed, int quantity)
        {
            string url = $"https://api.thecatapi.com/v1/images/search?breed_ids={breed}&limit={quantity}";
            return await httpClient.GetStringAsync(url);
        }
    }
}
