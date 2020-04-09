using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using WebImage.Context;
using WebImage.DBContext;

namespace WebImage.Models
{
    public class MyGalleries
    {
        private readonly IjpContext ijpContext;
        public List<ItemGallery> ItemGalleries { get; set; }

        public MyGalleries(IjpContext _ijpContext, string userId)
        {
            ijpContext = _ijpContext;
            LoadGallery(userId);
        }

        public MyGalleries()
        {
            ItemGalleries = new List<ItemGallery>();
        }


        public string AttributeChecked(string columns, string columnName)
        {
            string columnsList = Helper.Decode(columns);
            var isChecked = columnsList.Contains(columnName) ? "" : "checked";
            return isChecked;
        }

        public void RemoveGallery(int galleryId)
        {
            using TransactionScope scope = new TransactionScope();

            var galleryfile = ijpContext.GalleryFile.Where(x => x.GalleryId == galleryId);
            ijpContext.GalleryFile.RemoveRange(galleryfile);
            ijpContext.SaveChanges();

            var gallery = ijpContext.Gallery.FirstOrDefault(x => x.GalleryId == galleryId);
            ijpContext.Gallery.Remove(gallery);
            ijpContext.SaveChanges();

            scope.Complete();
        }

        private void LoadGallery(string userId)
        {
            ItemGalleries = new List<ItemGallery>();
            List<IjpGallery> myGallery = ijpContext.Gallery.Where(x => x.UserId == userId).ToList();

            foreach (var item in myGallery)
            {
                ItemGalleries.Add(new ItemGallery()
                {
                    Gallery = item,
                    GalleryFile = ijpContext.GalleryFile.Where(x => x.GalleryId == item.GalleryId).ToList()                    
                });
            }
        }

        public int SaveGallery(ItemGallery gallery, string description_ids, string userId)
        {
            int newId = 0;
            using TransactionScope scope = new TransactionScope();

            if (gallery.Gallery.GalleryId == 0)
            {
                if (!string.IsNullOrEmpty(gallery.Gallery.Images))
                {
                    var mygallery = new IjpGallery()
                    {
                        Name = gallery.Gallery.Name,
                        Description = gallery.Gallery.Description,
                        Columns = gallery.Gallery.Columns,
                        DateInsert = DateTime.Now,
                        DateUpdate = DateTime.Now,
                        UserId = userId,
                        Images = gallery.Gallery.Images                        
                    };

                    ijpContext.Gallery.Add(mygallery);
                    ijpContext.SaveChanges();
                    newId = mygallery.GalleryId;

                    var images = Helper.Decode(gallery.Gallery.Images);

                    foreach (string img in images.Split(','))
                    {
                        var imagename = img.Split('/')[4].Split('?')[0];
                        var file = ijpContext.File.FirstOrDefault(x => x.Name.Equals(imagename));

                        if (file != null)
                        {
                            ijpContext.GalleryFile.Add(new IjpGalleryFile
                            {
                                FileId = file.FileId,
                                GalleryId = mygallery.GalleryId,
                                Description = "description of " + imagename
                            });
                        }
                    }
                    ijpContext.SaveChanges();
                }
            }
            else
            {
                var mygallery = ijpContext.Gallery.FirstOrDefault(x => x.GalleryId == gallery.Gallery.GalleryId);
                newId = mygallery.GalleryId;

                if (mygallery != null)
                {
                    mygallery.Name = gallery.Gallery.Name;
                    mygallery.Description = gallery.Gallery.Description;
                    mygallery.DateUpdate = DateTime.Now;
                    mygallery.UserId = userId;
                    mygallery.Columns = gallery.Gallery.Columns;
                    mygallery.Images = gallery.Gallery.Images;
                    mygallery.Active = gallery.Gallery.Active;

                    ijpContext.SaveChanges();

                    var galleryfile = ijpContext.GalleryFile.Where(x => x.GalleryId == gallery.Gallery.GalleryId);

                    if (galleryfile != null)
                    {
                        foreach (string descrId in description_ids.Split(','))
                        {
                            var desc = descrId.Split("-")[0];
                            int id = Convert.ToInt32(descrId.Split("|")[1]);

                            galleryfile.FirstOrDefault(x => x.FileId == id).Description = descrId.Split("|")[0];
                        }
                        ijpContext.SaveChanges();
                    }
                }
            }

            scope.Complete();
            return newId;
        }
    };
}
