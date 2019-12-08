using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        public ContentModel ijpModel { get; set; }

        public string staticico { get; set; }
        private string Host { get; set; }

        public GalleryModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();

            //staticico= Path.Combine(hostingEnv.WebRootPath, "static", "video-play.png");
            staticico = Path.Combine(".", "static", "zoom.png");
        }

        public void OnGet(string bookmark)
        {
            ijpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext);

            if (!string.IsNullOrEmpty(bookmark))
                ijpModel.AddToSelection(bookmark);
        }


    }
}
