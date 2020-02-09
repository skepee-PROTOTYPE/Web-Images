using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Models;

namespace WebImage.Pages
{
    public class MyGalleriesModel : PageModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;
        private UserManager<IdentityUser> userManager { get; set; }
        public ContentModel IjpModel { get; set; }

        public MyGalleriesModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext, UserManager<IdentityUser> _userManager)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            userManager = _userManager;

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
        }

        public void OnGet(string bookmark, string sh, IdentityUser user, string gallery, string newg)
        {
            var user1 = new IdentityUser("skepee01@gmail.com");
            var userid = "bcdccebd-9f08-4485-96c3-cdea50240a4a";

            IjpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext, bookmark, sh, gallery);
            // if i am selecting new images, automatically i ll create an item in mysavedgallery
            if (newg == "y")
            {
                IjpModel.SaveGallery(0, "unNamed_" + DateTime.Now.ToString("yyyyMMdd"), "", userid);
            }

            var mydata = IjpModel.GetFileInfoJson(bookmark, sh);
        }

        public RedirectToPageResult OnPostSaveGallery(string bookmark, string sh)
        {
            int galleryId = 0;
            string gallery = "";
            string descr = "";
            if (Request.Form["inputgallerydescription"].Count == 1)
            {
                descr = Request.Form["inputgallerydescription"][0];
            }

            if (Request.Form["inputgalleryname"].Count == 1)
            {
                gallery = Request.Form["inputgalleryname"][0];
            }

            if (Request.Form["galleryid"].Count == 1)
            {
                galleryId = Int32.Parse(Request.Form["galleryid"][0]);
            }

            var user1 = new IdentityUser("skepee01@gmail.com");
            IjpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext, bookmark, sh);
            var userid = "bcdccebd-9f08-4485-96c3-cdea50240a4a";
            IjpModel.SaveGallery(galleryId, gallery, descr, userid);
            return new RedirectToPageResult("Gallery");
        }


    }
}