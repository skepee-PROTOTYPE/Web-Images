using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebImage.DBContext;
using WebImage.Models;

namespace WebImage.Controllers
{
    [Produces("application/json")]
    public class HomeController : Controller
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;
        private UserManager<IdentityUser> userManager { get; set; }


        private string Host { get; set; }

        public HomeController(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext, UserManager<IdentityUser> _userManager)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            userManager = _userManager;

            //TestData d = new TestData(hostingEnv, httpContextAccessor, ijpContext);
            //d.AddFromDirectory();
            //d.DownloadImages();

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();
        }


        [HttpPost("/api/gallery/{id}")]
        public JsonResult GetListSelection(int id)
        {
            if (id > 0)
            {
                //TODO : for now... to be removed when the userid isassigned by identityserver
                System.Security.Claims.ClaimsPrincipal currentUser = this.User;

                DateTime StartDate = DateTime.Now;

                var mycontainer = new MyContainer(httpContextAccessor, ijpContext, hostingEnv);
                mycontainer.myGalleries.LoadGallery(id, null, Host);

                string userId = mycontainer.myGalleries.Gallery[0].Gallery.UserId; //userManager.GetUserId(currentUser)

                mycontainer.myImages.LoadMyImages(userId, Host, hostingEnv.WebRootPath);

                var mydata = mycontainer.GetFileInfoJson();

                return Json(new JsonGallery()
                {
                    Data = mydata,

                    Stat = new JsonStats()
                    {
                        Count = mydata.MyJson.Count(),
                        TotalLengthKb = mydata.MyJson.Select(x => x.Length).Sum(),
                        ElapsedTime = (DateTime.Now - StartDate).TotalMilliseconds
                    }
                }); 
            }
            else
                return Json(new JsonGallery());
        }

    }
}
