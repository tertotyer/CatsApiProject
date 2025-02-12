using CatsTaskProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsTaskProject.Managers.Interfaces
{
    interface IBreedManager
    {
        public Task<Breed> GetBreedById(string breedId);
        public Task<IList<Breed>> GetBreeds(int quantity, int page);
        public Task<IList<Breed>> SearchBreedsByName(string text);

        public static abstract IList<Breed> FilterBreedCollectionByName(IList<Breed> breeds, string text);
        public static abstract IList<Breed> GetAllNewBreeds(IList<Breed> collection, IList<Breed> newCollection);
    }
}
