using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebImage.Context;

namespace WebImage.Models
{
    public class ContentModel
    {
        private readonly IjpContext _IjpContext;
        //private static List<FileModel> FilesInDirectory;
        public List<FileModel> MyFiles { get; set; }
        public string ApiGetUrl { get; set; }
        public string TypeSelected { get; set; }
        public string Profile { get; set; }
        public string OptionList { get; set; }

        public ContentModel(string host, IHostingEnvironment env, IjpContext IjpContext)
        {
            _IjpContext = IjpContext;
            MyFiles = new List<FileModel>();

            foreach (var x in _IjpContext.File)
            {
                MyFiles.Add(new FileModel()
                {
                    Category = x.Category,
                    Extension = x.Extension,
                    IsPrivate = false,
                    IsSelected = false,
                    LengthKB = x.LengthKB,
                    LengthMB = x.LengthMB,
                    Name = x.Name,
                    Title = x.Title,
                    Url = x.Url,
                    Content=x.Content
                });
            }


            //_IjpContext.FileContent.ForEachAsync(x => MyFiles.Add(new FileModel()
            //{
            //    Category = x.Category,
            //    Extension = x.Extension,
            //    IsPrivate = false,
            //    IsSelected = false,
            //    LengthKB = x.LengthKB,
            //    LengthMB = x.LengthMB,
            //    Name = x.Name,
            //    Title = x.Title,
            //    Url = x.Url
            //}));

        }

        public void AddToSelection(string MySelectedFiles)
        {
            if (!string.IsNullOrEmpty(MySelectedFiles))
            {
                foreach (string fileName in MySelectedFiles.Split(','))
                {
                    if (!string.IsNullOrEmpty(fileName))
                    {
                        MyFiles.FirstOrDefault(x => x.Name.Equals(fileName, StringComparison.InvariantCultureIgnoreCase)).IsSelected = true;
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
        //public string Path { get; set; }
        public bool IsSelected { get; set; }
        //public bool IsPrivate { get; set; }
    }

    //public class FileModel : JsonModel
    //{
    //    public string Name { get; set; }
    //    public string Extension { get; set; }
    //    public string Path { get; set; }
    //    public bool IsSelected { get; set; }
    //    public bool IsPrivate { get; set; }
    //    public string Category { get; set; }
    //}

    //public class JsonModel
    //{
    //    public string Title { get; set; }
    //    public double LengthKb { get; set; }
    //    public double LengthMb { get; set; }
    //    public string Url { get; set; }
    //}

    public class JsonData
    {
        public List<MyData> MyData { get; set; }
        public Statistics Stat { get; set; }

        public JsonData()
        {
            MyData = new List<MyData>();
        }
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
