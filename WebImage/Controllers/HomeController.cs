using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebImage.Context;
using WebImage.Models;

namespace WebImage.Controllers
{
    public class HomeController : Controller
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHostingEnvironment hostingEnv;

        private string Host { get; set; }

        public HomeController(IHostingEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
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

        //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client, NoStore = false)]
        public IActionResult GenerateJson()
        {
            ContentModel myFiles = new ContentModel(Host, hostingEnv, ijpContext);
            return View(myFiles);
        }

        public IActionResult ClientCarousel(string pars)
        {
            ContentModel myFiles = new ContentModel(Host, hostingEnv, ijpContext);

            var mydata = FilterMyFiles(pars);
            mydata.MyJson.ForEach(x => myFiles.MyFiles.Add(
                 new FileModel
                 {
                     Category = x.Category,
                     Content = x.Content,
                     Extension = x.Extension,
                     IsPrivate = x.IsPrivate,
                     IsSelected = true,
                     LengthKB = x.LengthKB,
                     LengthMB = x.LengthMB,
                     Name = x.Name,
                     Title = x.Title,
                     Url = x.Url
                 })
            );
            return View("ClientCarousel", myFiles);
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult RemoveFromSelectionList(string pars)
        {
            ContentModel myFiles = new ContentModel(Host, hostingEnv, ijpContext);
            if (!string.IsNullOrEmpty(pars))
            {
                string decpars = Helper.Decode(pars);

                string SelectedFiles = decpars.Split("|")[0];
                string decSelectedFiles = decpars.Split("|")[1];
                myFiles.TypeSelected = decpars.Split("|")[2];
                myFiles.Profile = decpars.Split("|")[3];

                myFiles.AddToSelection(SelectedFiles);
                myFiles.RemoveFromSelection(decSelectedFiles);

                var sel2 = string.Join(",", SelectedFiles.Split(',').Where(x => (!x.Equals(decSelectedFiles))));
                var pars2 = Helper.Encode(sel2 + "|" + myFiles.TypeSelected + "|" + myFiles.Profile);

                myFiles.ApiGetUrl = Request.Scheme + "://" + Request.Host + "/api/getselection/" + pars2;
            }
            return View("GenerateJson", myFiles);
        }

        public IActionResult AddToSelectionList(string pars)
        {
            ContentModel myFiles = new ContentModel(Host, hostingEnv, ijpContext);
            if (!string.IsNullOrEmpty(pars))
            {
                string decpars = Helper.Decode(pars);

                string decSelectedFiles = decpars.Split("|")[0];
                myFiles.TypeSelected = decpars.Split("|")[1];
                myFiles.Profile = decpars.Split("|")[2];

                myFiles.AddToSelection(decSelectedFiles);
                myFiles.ApiGetUrl = Request.Scheme + "://" + Request.Host + "/api/getselection/" + pars;
            }
            return View("GenerateJson", myFiles);
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


        private MyData FilterMyFiles(string pars)
        {
            List<FileModel> files = new List<FileModel>();
            string decodedString = Helper.Decode(pars);
            ContentModel myfiles = new ContentModel(Host, hostingEnv, ijpContext);

            if (!string.IsNullOrEmpty(decodedString))
            {
                string decSelectedFiles = decodedString.Split("|")[0];
                //string TypeSelected = decodedString.Split("|")[1];
                string Profile = decodedString.Split("|")[2];

                files = myfiles.MyFiles.Where(x => decSelectedFiles.Split(",").Contains(x.Name)).ToList();

                return new MyData()
                {
                    MyJson = files.Select(
                    x => new IjpFile()
                    {
                        Url = x.Url,
                        Title = x.Title,
                        LengthKB = x.LengthKB,
                        LengthMB = x.LengthMB,
                        Category=x.Category,
                        Content=x.Content,
                        Extension=x.Extension,
                        IsPrivate=x.IsPrivate,
                        Name=x.Name
                    }).ToList(),

                    Profile = Profile
                };
            }
            else
            {
                return null;
            }
        }


        [HttpGet("/api/getselection/{pars}")]
        public JsonResult GetListSelection(string pars)
        {
            DateTime StartDate = DateTime.Now;

            var mydata = FilterMyFiles(pars);

            return Json(new JsonData()
            {
                MyData = new List<MyData>()
                    {
                        mydata
                    },

                Stat = new Statistics()
                {
                    Count = mydata.MyJson.Count(),
                    TotalLengthKb = mydata.MyJson.Select(x => x.LengthKB).Sum(),
                    TotalLengthMb = mydata.MyJson.Select(x => x.LengthMB).Sum(),
                    ElapsedTime = (DateTime.Now - StartDate).TotalDays
                }
            });
        }

    }
}
