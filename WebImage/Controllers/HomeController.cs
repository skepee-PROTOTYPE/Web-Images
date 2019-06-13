using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebImage.Models;

namespace WebImage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHostingEnvironment hostingEnv;
        private string Host { get; set; }

        public HomeController(IHostingEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();
        }

        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        public IActionResult Index()
        {
            ContentModel files = new ContentModel(Host);

            return View(files);
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

        //[HttpGet("/api/maxlen/{MaxLengthKB}")]
        [HttpGet("/api")]
        public JsonResult GetList(decimal MaxLengthKB)
        {
            DateTime StartDate = DateTime.Now;

            ContentModel myfiles = new ContentModel(Host);
            List<FileModel> files = new List<FileModel>();

            if (MaxLengthKB > 0)
            {
                files = myfiles.Files.Where(x => x.LengthKb <= MaxLengthKB).ToList();
            }
            else
            {
                files = myfiles.Files;
            }

            var myjson = files.Select(
                x => new JsonModel()
                {
                    Url = x.Url,
                    Title = x.Title,
                    LengthKb = x.LengthKb,
                    LengthMb = x.LengthMb
                }).ToList();


            var c = new Statistics()
            {
                Count = files.Count(),
                TotalLengthKb = files.Select(x => x.LengthKb).Sum(),
                TotalLengthMb = files.Select(x => x.LengthMb).Sum(),
                ElapsedTime=  (DateTime.Now-StartDate).TotalDays

            };

            return Json(new JsonData()
            {
                MyJson=myjson,
                Stat=c
            });
        }





    }
}
