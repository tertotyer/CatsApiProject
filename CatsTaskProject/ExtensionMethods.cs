using CatsTaskProject.Managers;
using CatsTaskProject.Models;

namespace CatsTaskProject
{
    internal static class ExtensionMethods
    {
        public static void AddImage(this Breed breed, CatImage image)
        {
            ImageManager imageManager = new();

            breed.MainImage = image;
            breed.MainImageId = image.Id;
            breed.MainImage.LocalImagePath = imageManager.GetImagePath(breed.MainImage.Url);
        }
    }
}
