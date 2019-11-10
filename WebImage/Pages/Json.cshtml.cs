using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Models;

namespace WebImage.Pages
{
    public class JsonModel : PageModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        public ContentModel ijpModel { get; set; }
        private string Host { get; set; }

        public JsonModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();
        }

        public void OnGet()
        {
            ijpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext);
        }
    }
}
