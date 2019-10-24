using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebImage.Models;

namespace WebImage.Context
{
    public class TestData
    {
        private readonly IjpContext _IjpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHostingEnvironment hostingEnv;
        private static string host;

        public List<IjpContext> files { get; set; }

        public TestData(IHostingEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext ijpContext)
        {
            _IjpContext = ijpContext;
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            var request = httpContextAccessor.HttpContext.Request;
            host = request.Host.ToString();
        }

        public void AddFromDirectory()
        {
            string path = Path.Combine(hostingEnv.WebRootPath, "imagefolder", "public");
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo f = new FileInfo(file);

                    var sizekb = Math.Round(((double)f.Length) / 1024, 1);
                    var sizeMb = Math.Round(sizekb / 1024, 2);

                    _IjpContext.File.Add(new IjpFile()
                    {
                        Title = f.Name.Replace("@", "").Replace(".", ""),
                        Name = f.Name.Replace("@", "").Replace(".", ""),
                        Extension = f.Extension,
                        LengthKB = sizekb,
                        LengthMB = sizeMb,
                        CategoryId = 1,
                        Content = Helper.byteFile(path + "/" + f.Name),
                        //Path = "/images/" + ((isPrivate) ? "private/" : "public/") + f.Name,
                        Url = host + "/images/" + f.Name
                        //IsPrivate = isPrivate
                    });
                }
                _IjpContext.SaveChanges();
            }
        }

        public void DownloadImages()
        {
            var list = _IjpContext.File.ToList();

            foreach (var file in list)
            {
                Helper.DownloadFile(file.Content, file.Name + file.Extension);
            }
        }
    }
}
