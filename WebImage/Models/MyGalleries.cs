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
        public List<ItemGallery> Gallery { get; set; }

        public MyGalleries(IjpContext _ijpContext)
        {
            ijpContext = _ijpContext;
            Gallery = new List<ItemGallery>();
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

        public List<ItemGallery> LoadGallery(int galleryId, string userId)
        {
            foreach (var item in ijpContext.Gallery.Where(x => x.UserId == userId))
            {
                Gallery.Add(new ItemGallery()
                {
                    Gallery = item,
                    GalleryFile = ijpContext.GalleryFile.Where(x => x.GalleryId == item.GalleryId).ToList(),
                    IsSelected = (galleryId > 0) ? item.GalleryId == galleryId : false
                });

            }

            return Gallery;
        }

        public void SaveGallery(ItemGallery gallery, string description_ids, string userId)
        {
            using TransactionScope scope = new TransactionScope();

            if (gallery.Gallery.GalleryId == 0)
            {
                if (!string.IsNullOrEmpty(gallery.Gallery.Images))
                {
                    var mygallery = new IjpGallery()
                    {
                        Name = gallery.Gallery.Name,
                        Description = gallery.Gallery.Description,
                        Url = gallery.Gallery.Url,
                        Columns = gallery.Gallery.Columns,
                        DateInsert = DateTime.Now,
                        DateUpdate = DateTime.Now,
                        UserId = userId,
                        Images = gallery.Gallery.Images
                    };

                    ijpContext.Gallery.Add(mygallery);
                    ijpContext.SaveChanges();

                    var images = Helper.Decode(gallery.Gallery.Images);

                    foreach (string img in images.Split(','))
                    {
                        var file = ijpContext.File.FirstOrDefault(x => x.Name.Equals(img));

                        if (file != null)
                        {
                            ijpContext.GalleryFile.Add(new IjpGalleryFile
                            {
                                FileId = file.FileId,
                                GalleryId = mygallery.GalleryId,
                                Description = "description of " + img
                            });
                        }
                    }
                    ijpContext.SaveChanges();
                }
            }
            else
            {
                var mygallery = ijpContext.Gallery.FirstOrDefault(x => x.GalleryId == gallery.Gallery.GalleryId);

                if (mygallery != null)
                {
                    mygallery.Name = gallery.Gallery.Name;
                    mygallery.Description = gallery.Gallery.Description;
                    mygallery.DateUpdate = DateTime.Now;
                    mygallery.UserId = userId;
                    mygallery.Url = gallery.Gallery.Url;
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
                            int id = Convert.ToInt32(descrId.Split("-")[1]);

                            galleryfile.FirstOrDefault(x => x.FileId == id).Description = desc;
                        }
                        ijpContext.SaveChanges();
                    }
                }
            }

            scope.Complete();
        }
    };
}
