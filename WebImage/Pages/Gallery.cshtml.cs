using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Models;

namespace WebImage.Pages
{
    public class GalleryModel : PageModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        public ContentModel IjpModel { get; set; }

        public GalleryModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
        }


        public IActionResult OnGetSaveGallery()
        {
            return Page();
        }

        public void OnGet(string bookmark, string sh)
        {
            IjpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext,bookmark,sh);
            var mydata = IjpModel.GetFileInfoJson(bookmark, sh);
        }
    }
}
