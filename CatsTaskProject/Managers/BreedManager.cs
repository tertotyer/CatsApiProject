using CatsTaskProject.Managers.Interfaces;
using CatsTaskProject.Models;
using System.Text.Json;
using System.Linq;
using DynamicData;
using System.Drawing;
using System.Runtime.Caching;

namespace CatsTaskProject.Managers
{
    internal class BreedManager : IBreedManager
    {
        public BreedManager()
        {
        }

        public async Task<Breed> GetBreedById(string breedId)
        {
            CatAPIManager apiManager = CatAPIManager.Instance;
            string jsonResult = await apiManager.GetBreedById(breedId);

            return JsonSerializer.Deserialize<Breed>(jsonResult);
        }

        public async Task<IList<Breed>> GetBreeds(int quantity, int page)
        {
            CatAPIManager apiManager = CatAPIManager.Instance;
            string jsonResult = await apiManager.GetQuantityBreeds(quantity, page);

            return JsonSerializer.Deserialize<IList<Breed>>(jsonResult);
        }

        public async Task<IList<Breed>> SearchBreedsByName(string text)
        {
            CatAPIManager apiManager = CatAPIManager.Instance;
            string jsonResult = await apiManager.SearchBreedsByName(text);

            return JsonSerializer.Deserialize<IList<Breed>>(jsonResult);
        }

        public static IList<Breed> FilterBreedCollectionByName(IList<Breed> breeds, string text)
        {
            var containsBreeds = breeds.Where(x => x.Name.Contains(text, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Name);
            var startBreeds = containsBreeds.Where(x => x.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToList();

            return startBreeds.Union(containsBreeds).DistinctBy(x => x.Id).ToList();
        }

        public static IList<Breed> FilterCacheBreedsByName(string text)
        {
            var containsBreeds = MemoryCache.Default.Where(x => ((Breed)x.Value)?.Name.Contains(text, StringComparison.OrdinalIgnoreCase) ?? false).
                Select(x => (Breed)x.Value).OrderBy(x => x.Name);
            var startBreeds = containsBreeds.Where(x => x.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToList();

            return startBreeds.Union(containsBreeds).DistinctBy(x => x.Id).ToList();
        }

        public static IList<Breed> FilterBreedCollectionByOrigins(IList<Breed> breeds, IList<string> countries)
        {
            return breeds.Where(breed => countries.Any(country => country == breed.Origin)).ToList(); ;
        }

        public static IList<Breed> GetAllNewBreeds(IList<Breed> collection, IList<Breed> newCollection)
        {
            var distinctCollection = collection.Union(newCollection).DistinctBy(x => x.Id).ToList();
            int distinctElemtsCount = distinctCollection.Count - collection.Count;

            return distinctCollection[(distinctCollection.Count - distinctElemtsCount)..^0];
        }
    }
}
