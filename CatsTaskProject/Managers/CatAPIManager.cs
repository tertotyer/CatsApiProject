using Microsoft.Extensions.Configuration;
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
            string apiKey = App.Configuration["x-api-key"];
            _httpClient.DefaultRequestHeaders.Add("x-api-key", apiKey);
        }

        public async Task<string> GetBreeds()
        {
            string url = $"http://api.thecatapi.com/v1/breeds";
            await Task.Delay(50);
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetQuantityBreeds(int quantity, int page)
        {
            string url = $"http://api.thecatapi.com/v1/breeds?limit={quantity}&page={page}";
            await Task.Delay(50);
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreedById(string id)
        {
            string url = $"http://api.thecatapi.com/v1/breeds/{id}";
            await Task.Delay(50);
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> SearchBreedsByName(string searchString, bool attachImage = true)
        {
            string url = $"http://api.thecatapi.com/v1/breeds/search?q={searchString}&attach_image={attachImage}";
            await Task.Delay(50);
            return await _httpClient.GetStringAsync(url);
        }


        public async Task<string> GetImageById(string imageId)
        {
            string url = $"https://api.thecatapi.com/v1/images/{imageId}";
            await Task.Delay(50);
            return await _httpClient.GetStringAsync(url);
        }

        public async Task<string> GetBreedImages(string breedId, int quantity)
        {
            string url = $"https://api.thecatapi.com/v1/images/search?breed_id={breedId}&limit={quantity}&has_breeds=true";
            await Task.Delay(50);
            return await _httpClient.GetStringAsync(url);
        }
    }
}
