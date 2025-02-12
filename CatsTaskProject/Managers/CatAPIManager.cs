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

        private readonly HttpClient _httpClient = new();
        
        private CatAPIManager()
        {
        }

        public async Task<string> GetBreeds(int quantity, int page)
        {
            string url = $"http://api.thecatapi.com/v1/breeds?limit={quantity}&page={page}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreedById(string id)
        {
            string url = $"http://api.thecatapi.com/v1/breeds/{id}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> SearchBreedsByName(string searchString, bool attachImage = true)
        {
            string url = $"http://api.thecatapi.com/v1/breeds/search?q={searchString}&attach_image={attachImage}";
            return await _httpClient.GetStringAsync(url);
        }


        public async Task<string> GetImageById(string imageId)
        {
            string url = $"https://api.thecatapi.com/v1/images/{imageId}";
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreedImages(string breedId, int quantity)
        {
            string url = $"https://api.thecatapi.com/v1/images/search?breed_ids={breedId}&limit={quantity}";
            return await _httpClient.GetStringAsync(url);
        }
    }
}
