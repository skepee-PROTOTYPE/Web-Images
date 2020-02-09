using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using WebImage.Models;

namespace WebImage.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;
        private UserManager<IdentityUser> userManager { get; set; }

        public ContentModel IjpModel { get; set; }

        public GalleryModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext, UserManager<IdentityUser> _userManager)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            userManager = _userManager;

            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            //var user = userManager.GetUser(currentUser);

        }

        public void OnGet(string bookmark, string sh, IdentityUser user, string gallery)
        {
            var user1 =  new IdentityUser("skepee01@gmail.com");

            IjpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext,bookmark,sh,gallery);
            var mydata = IjpModel.GetFileInfoJson(bookmark, sh);
        }
    }
}
