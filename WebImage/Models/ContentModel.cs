using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;
using WebImage.Context;
using WebImage.DBContext;

namespace WebImage.Models
{
    public class ContentModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        public List<IjpCategory> Category { get; set; }
        //public List<MyGallery> MySavedGalleries { get; set; }
       // public static List<FileModel> MyFiles { get; set; }

        //public static SiteImages SiteImage;

        public string TypeSelected { get; set; }
        public string ApiUrl { get; set; }
        public string HideAttributeList { get; set; }
        public string Bookmark { get; set; }

        string Host { get; set; }


        public ContentModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext, int galleryId = 0)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();

            // MySavedGalleries = new List<MyGallery>();

            //foreach (var item in ijpContext.Gallery)
            //{
            //    MySavedGalleries.Add(new MyGallery()
            //    {
            //        Gallery = item,
            //        GalleryFile = ijpContext.GalleryFile.Where(x => x.GalleryId == item.GalleryId).ToList(),
            //        IsSelected = item.GalleryId == galleryId,
            //        JsonGallery = this.GetFileInfoJson(item.Images, item.Columns)
            //    });
            //}

            //if (galleryId > 0)
            //{
            //    Bookmark = MySavedGalleries.FirstOrDefault(x => x.IsSelected).Gallery.Images;
            //    HideAttributeList = MySavedGalleries.FirstOrDefault(x => x.IsSelected).Gallery.Columns;
            //    ApiUrl = MySavedGalleries.FirstOrDefault(x => x.IsSelected).Gallery.Url;
            //}


            Category = new List<IjpCategory>();
            Category.AddRange(ijpContext.Category.OrderBy(x => x.Name));


       //     InitSiteImages();

        }


  




   



        public string DisplayInfo(string columnName)
        {
            string columnsList = Helper.Decode(HideAttributeList);
            var isvisible = columnsList.Contains(columnName) ? "none" : "block";
            return isvisible;
        }


        //public void AddToSelection(string MySelectedFiles)
        //{
        //    if (!string.IsNullOrEmpty(MySelectedFiles))
        //    {
        //        foreach (string fileName in MySelectedFiles.Split(','))
        //        {
        //            if (!string.IsNullOrEmpty(fileName))
        //            {
        //                MyFiles.FirstOrDefault(x => x.Name.Equals(fileName)).IsSelected = true;
        //            }
        //        }
        //    }
        //}

        //public void RemoveFromSelection(string MySelectedFiles)
        //{
        //    if (!string.IsNullOrEmpty(MySelectedFiles))
        //    {
        //        MyFiles.FirstOrDefault(x => x.Name.Equals(MySelectedFiles, StringComparison.InvariantCultureIgnoreCase)).IsSelected = false;
        //    }
        //}

    }
}
