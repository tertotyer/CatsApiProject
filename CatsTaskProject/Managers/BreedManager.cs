using CatsTaskProject.Managers.Interfaces;
using CatsTaskProject.Models;
using System.Text.Json;
using System.Linq;
using DynamicData;
using System.Drawing;

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
            string jsonResult = await apiManager.GetBreeds(quantity, page);

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
            return breeds.Where(x => x.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).OrderBy(x => x.Name).ToList();
        }

        public static IList<Breed> GetAllNewBreeds(IList<Breed> collection, IList<Breed> newCollection)
        {
            var distinctCollection = collection.Union(newCollection).DistinctBy(x => x.Id).ToList();
            int distinctElemtsCount = distinctCollection.Count - collection.Count;

            return distinctCollection[(distinctCollection.Count - distinctElemtsCount)..^0];
        }
    }
}
