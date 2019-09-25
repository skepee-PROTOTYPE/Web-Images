using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        public IActionResult GenerateJson()
        {
            ContentModel myFiles = new ContentModel(Host, hostingEnv);
            myFiles.GetFiles();
            return View(myFiles);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddToSelectionList(string selectedFiles)
        {
            ContentModel myFiles = new ContentModel(Host, hostingEnv);
            myFiles.AddFileToSelection(selectedFiles);
            myFiles.GetFiles();
            return View("GenerateJson",myFiles);
        }

        public IActionResult Admin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("/api/getmax/{MaxLengthKB}")]
        public JsonResult GetList(decimal MaxLengthKB)
        {
            DateTime StartDate = DateTime.Now;

            ContentModel myfiles = new ContentModel(Host, hostingEnv);
            List<FileModel> files = new List<FileModel>();

            if (MaxLengthKB > 0)
            {
                files = myfiles.UnselectedFiles.Where(x => x.LengthKb <= MaxLengthKB).ToList();
            }
            else
            {
                files = myfiles.UnselectedFiles;
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


        [HttpGet("/api/getselection/{selectedImages}")]
        public JsonResult GetListSelection(string selectedImages)
        {

            byte[] data = Convert.FromBase64String(selectedImages);
            string decodedString = Encoding.UTF8.GetString(data);

            DateTime StartDate = DateTime.Now;

            ContentModel myfiles = new ContentModel(Host, hostingEnv);
            myfiles.GetFiles();
            List<FileModel> files = new List<FileModel>();

            if (!string.IsNullOrEmpty(decodedString))
            {
                files = myfiles.UnselectedFiles.Where(x => decodedString.Split(",").Contains(x.Name)).ToList();
            }
            else
            {
                files = myfiles.UnselectedFiles;
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
                ElapsedTime = (DateTime.Now - StartDate).TotalDays

            };

            return Json(new JsonData()
            {
                MyJson = myjson,
                Stat = c
            });
        }

    }
}
