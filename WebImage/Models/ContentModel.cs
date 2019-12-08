using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WebImage.Context;

namespace WebImage.Models
{
    public class ContentModel
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;

        public List<FileModel> MyFiles { get; set; }
        public string ApiGetUrl { get; set; }
        public string TypeSelected { get; set; }
        public string OptionList { get; set; }
        public List<IjpCategory> Category { get; set; }
        public IjpUser User { get; set; }

        public ContentModel(IWebHostEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            MyFiles = new List<FileModel>();
            TestData td = new TestData(hostingEnv, httpContextAccessor, ijpContext);
            MyFiles = td.AddFromDirectory();

            ////string ss = "";
            //foreach (var x in ijpContext.File)
            //{ 
            ////string path = Path.Combine(hostingEnv.WebRootPath, "imagefolder");

            ////foreach (var file in Directory.GetFiles(path))
            ////{
            //    //FileInfo x = new FileInfo(file);

            //    //using Bitmap b = new Bitmap(Path.Combine(hostingEnv.WebRootPath, "imagefolder", x.Name));

            //    //ss += "('" + x.Name + "','" + x.Name + "','" + b.RawFormat + "'," + Math.Round(((double)x.Length) / 1024, 1) + ",null,0," + ((b.Width > b.Height) ? "1" : "0") + ",'" + b.PixelFormat + "'," + b.Width + "," + b.Height + "," + b.HorizontalResolution + "," + b.VerticalResolution + "),";

            //    MyFiles.Add(new FileModel()
            //    {
            //        CategoryId = x.CategoryId,
            //        RawFormat = x.RawFormat,
            //        IsPrivate = x.IsPrivate,
            //        IsLandscape = x.IsLandscape,
            //        LengthKB = x.LengthKB,
            //        Name = x.Name.CleanName(),
            //        Title = x.Title.CleanName(),
            //        Width = x.Width,
            //        Height = x.Height,
            //        HorizontalResolution = x.HorizontalResolution,
            //        VerticalResolution = x.VerticalResolution,
            //        PixelFormat = x.PixelFormat,
            //        IsSelected = false,
            //        Path = Path.Combine(".", "imagefolder", x.Name)
            //    });
            //}

            //Category = new List<IjpCategory>();
            //Category.AddRange(ijpContext.Category.OrderBy(x => x.Name));
        }

        public void AddToSelection(string MySelectedFiles)
        {
            if (!string.IsNullOrEmpty(MySelectedFiles))
            {
                foreach (string fileName in MySelectedFiles.Split(','))
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        MyFiles.FirstOrDefault(x => x.Name.Equals(fileName)).IsSelected = true;
                    }
                }
            }
        }

        public void RemoveFromSelection(string MySelectedFiles)
        {
            if (!string.IsNullOrEmpty(MySelectedFiles))
            {
                MyFiles.FirstOrDefault(x => x.Name.Equals(MySelectedFiles, StringComparison.InvariantCultureIgnoreCase)).IsSelected = false;
            }
        }

    }


    public class FileModel : IjpFile
    {
        public bool IsSelected { get; set; }
        public string Path { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
    }


    public class JsonData
    {
        public MyData MyData { get; set; }
        public Statistics Stat { get; set; }
    }


    public class MyData
    {
        public List<IjpFile> MyJson { get; set; }
        public string Profile { get; set; }

        public MyData()
        {
            MyJson = new List<IjpFile>();
        }
    }

    public class Statistics
    {
        public int Count { get; set; }
        public double TotalLengthKb { get; set; }
        public double TotalLengthMb { get; set; }
        public double ElapsedTime { get; set; }
    }
}
