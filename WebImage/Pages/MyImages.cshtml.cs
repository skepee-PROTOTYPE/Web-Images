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
        public MyImages myImages { get; set; }

        public GalleryModel(IjpContext _ijpContext, IHttpContextAccessor _httpContextAccessor)
        {
            httpContextAccessor = _httpContextAccessor;
            string userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier")).Value;
            myImages = new MyImages(_ijpContext, userId);
        }

        public void OnGet(bool isPrivate)
        {
            System.Security.Claims.ClaimsPrincipal currentUser = this.User;
            myImages.Images = myImages.Images.Where(x => x.IsPrivate == isPrivate).ToList();
        }
    }
}
