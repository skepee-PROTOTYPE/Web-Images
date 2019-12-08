using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private readonly IWebHostEnvironment hostingEnv;
        private static string host;

        //public List<IjpContext> files { get; set; }

        public TestData(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext ijpContext)
        {
            _IjpContext = ijpContext;
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            var request = httpContextAccessor.HttpContext.Request;
            host = request.Host.ToString();
        }

        //public void AddFromDirectory()
        //{
        //    string path = Path.Combine(hostingEnv.WebRootPath, "imagefolder", "public");
        //    if (Directory.Exists(path))
        //    {
        //        foreach (var file in Directory.GetFiles(path))
        //        {
        //            FileInfo f = new FileInfo(file);

        //            var sizekb = Math.Round(((double)f.Length) / 1024, 1);
        //            var sizeMb = Math.Round(sizekb / 1024, 2);

        //            _IjpContext.File.Add(new IjpFile()
        //            {
        //                Title = f.Name.CleanName(),
        //                Name = f.Name.CleanName(),
        //                //Extension = f.Extension,
        //                LengthKB = sizekb,
        //                //LengthMB = sizeMb,
        //                CategoryId = 1,
        //                //Content = Helper.byteFile(path + "/" + f.Name),
        //                //Path = "/images/" + ((isPrivate) ? "private/" : "public/") + f.Name,
        //                //Url = host + "/images/" + f.Name
        //                //IsPrivate = isPrivate
        //            });
        //        }
        //        _IjpContext.SaveChanges();
        //    }
        //}


        public List<FileModel> AddFromDirectory()
        {
            List<FileModel> list = new List<FileModel>();
            string path = Path.Combine(hostingEnv.WebRootPath, "imagefolder");
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo f = new FileInfo(file);

                    var sizekb = Math.Round(((double)f.Length) / 1024, 1);
                    //var sizeMb = Math.Round(sizekb / 1024, 2);


                    using Bitmap b = new Bitmap(Path.Combine(hostingEnv.WebRootPath, "imagefolder", f.Name));

                    //Bitmap resized = new Bitmap(b, new Size(b.Width / 4, b.Height / 4));
                    Bitmap resized = new Bitmap(b, new Size(200, 200));
                    string thumbName = f.Name.Split(".")[0] + "_thumb" + f.Extension;

                    if (!File.Exists(Path.Combine(f.Directory.ToString(), "Thumbs", thumbName)))
                        resized.Save(Path.Combine(f.Directory.ToString(), "Thumbs", thumbName));

                    //ss += "('" + x.Name + "','" + x.Name + "','" + b.RawFormat + "'," + Math.Round(((double)x.Length) / 1024, 1) + ",null,0," + ((b.Width > b.Height) ? "1" : "0") + ",'" + b.PixelFormat + "'," + b.Width + "," + b.Height + "," + b.HorizontalResolution + "," + b.VerticalResolution + "),";

                    //_IjpContext.File.Add(new IjpFile()
                    list.Add(new FileModel()
                    {
                        Name = f.Name,
                        Title = f.Name,
                        RawFormat = f.Extension,
                        LengthKB = sizekb,
                        CategoryId = 1,
                        Url = host + "/images/" + f.Name,
                        IsPrivate = false,
                        IsLandscape = ((b.Width > b.Height) ? true : false),
                        PixelFormat = b.PixelFormat.ToString(),
                        Width = b.Width,
                        Height = b.Height,
                        HorizontalResolution = b.HorizontalResolution,
                        VerticalResolution = b.VerticalResolution,
                        Path = Path.Combine(".", "imagefolder", f.Name),
                        Thumb = Path.Combine(".", "imagefolder", "Thumbs", thumbName)
                    });

                }
                //_IjpContext.SaveChanges();
            }

            return list;
        }



        public void DownloadImages()
        {
            var list = _IjpContext.File.ToList();

            foreach (var file in list)
            {
               // Helper.DownloadFile(file.Content, file.Name + file.Extension);
            }
        }
    }
}
