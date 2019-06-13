using System;
using System.Collections.Generic;
using System.IO;

namespace WebImage.Models
{
    public class ContentModel
    {
        public List<FileModel> Files { get; set; }
        
        public string MaxLen { get; set; }

        public ContentModel(string host)
        {
            string path= @"D:\imagefolder";

            Files = new List<FileModel>();
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo f = new FileInfo(file);

                    var sizekb = Math.Round(((decimal)f.Length) / 1024, 1);
                    var sizeMb = Math.Round(((decimal)sizekb) / 1024, 2);                    

                    Files.Add(new FileModel()
                    {
                        Title = string.Empty,
                        Name = f.Name,
                        Extension = f.Extension,
                        LengthKb =  sizekb,
                        LengthMb = sizeMb,
                        Path = "/images/" + f.Name,
                        Url = host +  "/images/" + f.Name
                    });
                }
            }
        }
    }



    public class FileModel: JsonModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
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
        public List<JsonModel> MyJson { get; set; }
        public Statistics Stat { get; set; }
    }

    public class Statistics
    {
        public int Count { get; set; }
        public decimal TotalLengthKb { get; set; }
        public decimal TotalLengthMb { get; set; }
        public double ElapsedTime { get; set; }
    }


}
