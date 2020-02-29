using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using WebImage.DBContext;
using WebImage.Models;

namespace WebImage.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;
        private readonly IjpContext ijpContext;
        private readonly UserManager<IdentityUser> userManager;
        public MyImages myImages { get; set; }
        string Host { get; set; }

        public GalleryModel(UserManager<IdentityUser> _userManager, IjpContext _ijpContext, IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _hostingEnv)
        {
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            userManager = _userManager;
            hostingEnv = _hostingEnv;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();

            myImages = new MyImages(_ijpContext);
        }

        public void OnGet(bool isPrivate)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;

            myImages.LoadMyImages(userManager.GetUserId(currentUser), Host, hostingEnv.WebRootPath);
            myImages.Images = myImages.Images.Where(x => x.IsPrivate == isPrivate).ToList();

        }
    }
}
