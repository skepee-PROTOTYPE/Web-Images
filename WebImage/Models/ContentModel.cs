﻿using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebImage.Models
{
    public class ContentModel
    {
        private static List<FileModel> FilesInDirectory;
        public List<FileModel> MyFiles { get; set; }

        public string ApiGetUrl { get; set; }

        public string MaxLen { get; set; }

        public ContentModel(string host, IHostingEnvironment env)
        {
            FilesInDirectory = GetFromDirectory(host, Path.Combine(env.WebRootPath, "imagefolder", "public"),false);
            FilesInDirectory.AddRange(GetFromDirectory(host, Path.Combine(env.WebRootPath, "imagefolder","private"),true));
            MyFiles = FilesInDirectory;
        }

        private List<FileModel> GetFromDirectory(string host, string path, bool isPrivate)
        {
            var myFiles = new List<FileModel>();

            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo f = new FileInfo(file);

                    var sizekb = Math.Round(((decimal)f.Length) / 1024, 1);
                    var sizeMb = Math.Round(sizekb / 1024, 2);

                    myFiles.Add(new FileModel()
                    {
                        Title = f.Name.Replace("@", "").Replace(".", ""),
                        Name = f.Name.Replace("@", "").Replace(".", ""),
                        Extension = f.Extension,
                        LengthKb = sizekb,
                        LengthMb = sizeMb,
                        Path = "/images/"+ ((isPrivate)?"private/":"public/") + f.Name,
                        Url = host + "/images/" + ((isPrivate) ? "private/" : "public/") + f.Name,
                        IsPrivate= isPrivate
                    });
                }
            }
            return myFiles;
        }


        //private JsonData GetJsonSelection()
        //{
        //    var myjson = this.SelectedFiles.Select(
        //        x => new JsonModel()
        //        {
        //            Url = x.Url,
        //            Title = x.Title,
        //            LengthKb = x.LengthKb,
        //            LengthMb = x.LengthMb
        //        }).ToList();

        //    return new JsonData()
        //    {
        //        MyJson = myjson
        //    };
        //}

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
    }



    public class FileModel : JsonModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public bool IsSelected { get; set; }
        public bool IsPrivate { get; set; }
        public string Category { get; set; }
    }

    public class JsonModel
    {
        public string Title { get; set; }
        public decimal LengthKb { get; set; }
        public decimal LengthMb { get; set; }
        public string Url { get; set; }
    }

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
        public List<JsonModel> MyJson { get; set; }
        public string Profile { get; set; }

        public MyData()
        {
            MyJson = new List<JsonModel>();
        }
    }

    public class Statistics
    {
        public int Count { get; set; }
        public decimal TotalLengthKb { get; set; }
        public decimal TotalLengthMb { get; set; }
        public double ElapsedTime { get; set; }
    }
}
