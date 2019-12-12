using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Context;
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
        public JsonData ImageJson { get; set; }
        public string x { get; set; }


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
            {
                ijpModel.AddToSelection(bookmark);
                //ImageJson = GetJson();
                ImageJson = new JsonData()
                {
                    MyData = new MyData
                    {
                        MyJson = ijpModel.MyFiles.Where(x => x.IsSelected).ToList()
                    }
                };
                x = JsonSerializer.Serialize(ImageJson);
            }
        }

        //[HttpPost("/api/gallery")]
        public JsonData GetJson()
        {
            string pars = "";
            DateTime StartDate = DateTime.Now;
            var mydata = FilterMyFiles(pars);

            return new JsonData()
            {
                MyData = mydata,

                //Stat = new Statistics()
                //{
                //    Count = mydata.MyJson.Count(),
                //    TotalLengthKb = mydata.MyJson.Select(x => x.LengthKB).Sum(),
                //   // TotalLengthMb = mydata.MyJson.Select(x => x.LengthMB).Sum(),
                //    ElapsedTime = (DateTime.Now - StartDate).TotalSeconds
                //}
            };
        }


        private MyData FilterMyFiles(string pars)
        {
            List<FileModel> files = new List<FileModel>();
            string decodedString = Helper.Decode(pars);
            ContentModel myfiles = new ContentModel(hostingEnv, httpContextAccessor, ijpContext);

            if (!string.IsNullOrEmpty(decodedString))
            {
                string decSelectedFiles = decodedString.Split("|")[0];
                //string TypeSelected = decodedString.Split("|")[1];
                string Profile = decodedString.Split("|")[2];

                string optionList = decodedString.Split("|")[3];
                bool addUrl = Convert.ToBoolean(optionList.Split(",")[0]);
                bool addEmbed = Convert.ToBoolean(optionList.Split(",")[1]);

                files = myfiles.MyFiles.Where(x => decSelectedFiles.Split(",").Contains(x.Name)).ToList();

                return new MyData()
                {
                    MyJson = files.Select(
                    x => new FileModel()
                    {
                        Url = (addUrl) ? x.Url : "",
                        Title = x.Title,
                        LengthKB = x.LengthKB,
                        CategoryId = x.CategoryId,
                        IsPrivate = x.IsPrivate,
                        Name = x.Name
                    }).ToList(),

                    Profile = Profile
                };
            }
            else
            {
                return null;
            }
        }




    }
}
