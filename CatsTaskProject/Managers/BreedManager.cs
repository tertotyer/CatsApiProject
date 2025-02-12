using CatsTaskProject.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsTaskProject.Managers
{
    internal class BreedManager
    {
        public BreedManager()
        {
        }

        public static ICollection<Breed> FilterBreedCollectionByName(ICollection<Breed> breeds, string text)
        {
            return breeds.Where(x => x.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase)).ToArray();
        }
    }
}
