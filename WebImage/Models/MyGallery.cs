using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.Models
{
    public class MyGallery
    {
        public IjpGallery Gallery { get; set; }
        public List<IjpGalleryFile> GalleryFile { get; set; }
        public bool IsSelected { get; set; }

        public MyGallery()
        {
            GalleryFile = new List<IjpGalleryFile>();
        }

        public bool IsImageInGallery(int fileId)
        {
            return GalleryFile.Where(x => x.FileId == fileId).Count() == 1;
        }

        public string GetDescription(int fileId)
        {
            return GalleryFile.FirstOrDefault(x => x.FileId == fileId).Description;
        }


    };

}
