using System.Collections.Generic;
using WebImage.Context;

namespace WebImage.Models
{
    public class ItemGallery
    {
        public IjpGallery Gallery { get; set; }
        public List<IjpGalleryFile> GalleryFile { get; set; }
        public bool IsSelected { get; set; }
        public JsonData JsonGallery { get; set; }

        public ItemGallery()
        {
            Gallery = new IjpGallery();
            GalleryFile = new List<IjpGalleryFile>();
            JsonGallery = new JsonData();
        }

        public void AddGallery(string name, string description, string images)
        {
            Gallery.GalleryId = 0;
            Gallery.Name = name;
            Gallery.Description = description;
            Gallery.Images = images;
            Gallery.Active = false;
        }
    }
}
