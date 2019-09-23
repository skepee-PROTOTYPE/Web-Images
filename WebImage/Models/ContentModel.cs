using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebImage.Models
{
    public class ContentModel
    {
        public List<FileModel> Files { get; set; }
        public List<FileModel> SelectedFiles { get; set; }
        private static List<FileModel> FilesInDirectory;
        public string SelectedListFiles { get; set; }

        public string MaxLen { get; set; }

        public ContentModel(string host, IHostingEnvironment env)
        {
            Files = new List<FileModel>();
            //SelectedFiles = new List<FileModel>();
            FilesInDirectory = new List<FileModel>();

            string path = Path.Combine(env.WebRootPath, "imagefolder");

            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo f = new FileInfo(file);

                    var sizekb = Math.Round(((decimal)f.Length) / 1024, 1);
                    var sizeMb = Math.Round(((decimal)sizekb) / 1024, 2);

                    FilesInDirectory.Add(new FileModel()
                    {
                        Title = string.Empty,
                        Name = f.Name,
                        Extension = f.Extension,
                        LengthKb = sizekb,
                        LengthMb = sizeMb,
                        Path = "/images/" + f.Name,
                        Url = host + "/images/" + f.Name
                    });
                }
            }
        }

        public void AddSelectedFile(string MySelectedFile)
        {
            if (SelectedFiles == null)
            {
                SelectedFiles = new List<FileModel>();
            }

            var elem = FilesInDirectory.FirstOrDefault(x => x.Name.Equals(MySelectedFile, StringComparison.InvariantCultureIgnoreCase));
            if (elem != null)
            {
                SelectedFiles.Add(elem);
            }
        }

        public void GetFiles()
        {
            if (SelectedFiles!=null && SelectedFiles.Any())
            {
                this.Files = FilesInDirectory.Where(x => !SelectedFiles.Contains(x)).ToList();
                this.SelectedFiles = SelectedFiles;
            }
            else
            {
                this.Files = FilesInDirectory.ToList();
            }
        }



    }





    public class FileModel : JsonModel
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
