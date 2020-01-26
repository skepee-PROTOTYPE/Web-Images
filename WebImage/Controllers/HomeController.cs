using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebImage.Models;

namespace WebImage.Controllers
{

    [Produces("application/json")]
    public class HomeController : Controller
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        private string Host { get; set; }

        public HomeController(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            //TestData d = new TestData(hostingEnv, httpContextAccessor, ijpContext);
            //d.AddFromDirectory();
            //d.DownloadImages();

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();
        }


        [HttpPost("/api/gallery")]
        public JsonResult GetListSelection(string imgs, string hc)
        {
            if (!string.IsNullOrEmpty(imgs))
            {
                DateTime StartDate = DateTime.Now;

                var IjpModel = new ContentModel(hostingEnv, httpContextAccessor, ijpContext, imgs, hc);
                var mydata = IjpModel.GetFileInfoJson(imgs, hc);

                return Json(new JsonData()
                {
                    MyData = mydata,

                    Stat = new Statistics()
                    {
                        Count = mydata.MyJson.Count(),
                        TotalLengthKb = mydata.MyJson.Select(x => x.Length).Sum(),                        
                        ElapsedTime = (DateTime.Now - StartDate).TotalMilliseconds
                    }
                }); ; ;
            }
            else
                return Json(new JsonData());
        }

    }
}
