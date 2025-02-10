using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;

namespace CatsTaskProject.Managers
{
    internal class ImageManager
    {
        private string _imageDirectory;

        public ImageManager()
        {
            _imageDirectory = Path.Combine(System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Images");
        }

        public string ImageDirectory
        {
            get => _imageDirectory;
        }

        public async Task LoadImage(string imageUrl)
        {
            string imagePath = GetImagePath(imageUrl);

            HttpClient client = new();
            var res = await client.GetAsync(imageUrl);

            byte[] bytes = await res.Content.ReadAsByteArrayAsync();

            using Image image = Image.FromStream(new MemoryStream(bytes));
            image.Save(imagePath); 
        }

        public bool ImageAlreadyLoadedById(string imageId)
        {
            DirectoryInfo root = new(_imageDirectory);
            string searchPattern = string.Concat(imageId, ".*");

            return root.GetFiles(searchPattern).Length > 0;
        }

        public bool ImageAlreadyLoadedByUrl(string imageUrl)
        {
            return File.Exists(GetImagePath(imageUrl));
        }

        private string GetImagePath(string imageUrl)
        {
            return Path.Combine(_imageDirectory, System.IO.Path.GetFileName(imageUrl));
        }
    }
}
