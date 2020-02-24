using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.DBContext;
using WebImage.Models;

namespace WebImage.Pages
{
    public class MyGalleriesModel : PageModel
    {
        private UserManager<IdentityUser> userManager { get; set; }
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        public MyContainer MyContainer { get; set; }
        private string Host { get; set; }

        public MyGalleriesModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext,UserManager<IdentityUser> _userManager)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            userManager = _userManager;

            MyContainer = new MyContainer(httpContextAccessor,ijpContext, hostingEnv);

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();
        }

        public void OnGet(int galleryId, string newg, string attr, string descrs)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            MyContainer.myImages.LoadMyImages(userManager.GetUserId(currentUser), Host,hostingEnv.WebRootPath);
            MyContainer.myGalleries.LoadGallery(galleryId, userManager.GetUserId(currentUser));

            if (!string.IsNullOrEmpty(newg))
            {
                ItemGallery mygallery = new ItemGallery();
                mygallery.AddGallery("NoName_" + DateTime.Now.ToString("yyyyMMdd"), "NoDescription_" + DateTime.Now.ToString("yyyyMMdd"), newg);                
                this.MyContainer.myGalleries.SaveGallery(mygallery, descrs, userManager.GetUserId(currentUser));
            }

            if (galleryId > 0 & !string.IsNullOrEmpty(attr))
            {
                MyContainer.myGalleries.Gallery.FirstOrDefault(x => x.Gallery.GalleryId == galleryId).Gallery.Columns = attr;
            }
        }

        public RedirectToPageResult OnPostRemoveGallery(int galleryId)
        {
            this.MyContainer.myGalleries.RemoveGallery(galleryId);
            return new RedirectToPageResult("MyGalleries");
        }


        private string GetRequestParam(string param)
        {
            if (Request.Form[param].Count == 1)
                return Request.Form[param][0];
            else
                return string.Empty;
        }

        public RedirectToPageResult OnPostRefreshGallery()
        {
            ItemGallery newgallery = new ItemGallery();
            newgallery.Gallery.Columns = Helper.Encode(GetRequestParam("attrs"));
            newgallery.Gallery.Url = GetRequestParam("inputgalleryurl");
            newgallery.Gallery.Description = GetRequestParam("inputgallerydescription");
            newgallery.Gallery.Name = GetRequestParam("inputgalleryname");
            newgallery.Gallery.GalleryId = Convert.ToInt32(GetRequestParam("galleryid"));
            newgallery.Gallery.Active = GetRequestParam("toggleActive") == "on" ? true : false;
            newgallery.Gallery.Images = Helper.Encode(GetRequestParam("images"));

            string description_ids = GetRequestParam("descriptions");

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            MyContainer.myImages.LoadMyImages(userManager.GetUserId(currentUser), Host, hostingEnv.WebRootPath);
            MyContainer.myGalleries.LoadGallery(newgallery.Gallery.GalleryId, userManager.GetUserId(currentUser));

            //this.MyContainer.myGalleries.SaveGallery(newgallery, description_ids, userManager.GetUserId(currentUser));
            return new RedirectToPageResult("MyGalleries");
        }



        public RedirectToPageResult OnPostSaveGallery()
        {
            ItemGallery newgallery = new ItemGallery();            
            newgallery.Gallery.Columns = Helper.Encode(GetRequestParam("attrs"));
            newgallery.Gallery.Url = GetRequestParam("inputgalleryurl");
            newgallery.Gallery.Description = GetRequestParam("inputgallerydescription");
            newgallery.Gallery.Name = GetRequestParam("inputgalleryname");
            newgallery.Gallery.GalleryId = Convert.ToInt32(GetRequestParam("galleryid"));
            newgallery.Gallery.Active = GetRequestParam("toggleActive") == "on" ? true : false;
            newgallery.Gallery.Images = Helper.Encode(GetRequestParam("images"));
            
            string description_ids = GetRequestParam("descriptions");

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            this.MyContainer.myGalleries.SaveGallery(newgallery, description_ids, userManager.GetUserId(currentUser));
            return new RedirectToPageResult("MyGalleries");
        }


        public void SaveGallery(ItemGallery newgallery, string listImages, string columns, string userId)
        {
            MyContainer.myGalleries.LoadGallery(newgallery.Gallery.GalleryId,userId);
            this.MyContainer.myGalleries.SaveGallery(newgallery, "", userId);
        }

    }
}