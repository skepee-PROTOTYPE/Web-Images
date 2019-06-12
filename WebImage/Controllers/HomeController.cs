using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using WebImage.Models;

namespace WebImage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment hostingEnv;

        public HomeController(IHostingEnvironment _hostingEnv)
        {
            hostingEnv = _hostingEnv;
        }

        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        public IActionResult Index()
        {
            ContentModel files = new ContentModel(@"C:\Users\miacone\source\repos\WebImage\WebImage\wwwroot\images");
            





            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
