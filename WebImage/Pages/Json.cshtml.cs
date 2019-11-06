using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Context;
using WebImage.Models;

namespace WebImage.Pages
{
    public class JsonModel : PageModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        [BindProperty]
        private string Host { get; set; }
        //[BindProperty]
        public List<FileModel> MyFiles { get; set; }
        [BindProperty]
        public string ApiGetUrl { get; set; }
        [BindProperty]
        public string TypeSelected { get; set; }
        [BindProperty]
        public List<IjpCategory> Category { get; set; }

        public JsonModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();
        }



        public async Task OnGet()
        {
            MyFiles = new List<FileModel>();

            foreach (var x in ijpContext.File)
            {
                MyFiles.Add(new FileModel()
                {
                    CategoryId = x.CategoryId,
                    Extension = x.Extension,
                    IsPrivate = x.IsPrivate,
                    IsSelected = false,
                    LengthKB = x.LengthKB,
                    LengthMB = x.LengthMB,
                    Name = x.Name,
                    Title = x.Title,
                    Url = x.Url,
                    Content = x.Content
                });
            }

            Category = new List<IjpCategory>();
            Category.AddRange(ijpContext.Category.OrderBy(x => x.Name));


            //ContentModel myFiles = new ContentModel(Host, hostingEnv, ijpContext);

            //var mydata = FilterMyFiles(string.Empty);
            //mydata.MyJson.ForEach(x => myFiles.MyFiles.Add(
            //     new FileModel
            //     {
            //         CategoryId = x.CategoryId,
            //         Content = x.Content,
            //         Extension = x.Extension,
            //         IsPrivate = x.IsPrivate,
            //         IsSelected = false,
            //         LengthKB = x.LengthKB,
            //         LengthMB = x.LengthMB,
            //         Name = x.Name,
            //         Title = x.Title,
            //         Url = x.Url
            //     })
            //);

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

                string optionList = decodedString.Split("|")[3];
                bool addUrl = Convert.ToBoolean(optionList.Split(",")[0]);
                bool addEmbed = Convert.ToBoolean(optionList.Split(",")[1]);

                files = myfiles.MyFiles.Where(x => decSelectedFiles.Split(",").Contains(x.Name)).ToList();

                return new MyData()
                {
                    MyJson = files.Select(
                    x => new IjpFile()
                    {
                        Url = (addUrl) ? x.Url : "",
                        Title = x.Title,
                        LengthKB = x.LengthKB,
                        LengthMB = x.LengthMB,
                        CategoryId = x.CategoryId,
                        Content = (addEmbed) ? x.Content : null,
                        Extension = x.Extension,
                        IsPrivate = x.IsPrivate,
                        Name = x.Name
                    }).ToList(),

                    Profile = Profile
                };
            }
            else
            {
                files = myfiles.MyFiles.ToList();

                return new MyData()
                {
                    MyJson = files.Select(
                    x => new IjpFile()
                    {
                        Url = x.Url,
                        Title = x.Title,
                        LengthKB = x.LengthKB,
                        LengthMB = x.LengthMB,
                        CategoryId = x.CategoryId,
                        Content = x.Content,
                        Extension = x.Extension,
                        IsPrivate = x.IsPrivate,
                        Name = x.Name
                    }).ToList(),

                    Profile = ""
                };
            }
        }
    }
}
