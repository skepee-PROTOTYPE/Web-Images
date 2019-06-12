using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using static System.Net.Mime.MediaTypeNames;
//using System.Drawing;
//using System.Windows.Media.Imaging;

namespace WebImage.Models
{
    public class ContentModel
    {
        public List<FileModel> Files { get; set; }


        public ContentModel(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    FileInfo f = new FileInfo(file);


                    //BitmapSource img = BitmapFrame.Create(fs);
                    //BitmapMetadata md = (BitmapMetadata)img.Metadata;


                    //Image img = Image.FromFile(fileName);
                    //BitmapSource img = BitmapFrame.Create(fs);

                    //using (Image sourceImage = Image.FromStream(stream, false, false))
                    //{
                    //    //Console.WriteLine(sourceImage.Width);
                    //    //Console.WriteLine(sourceImage.Height);
                    //}


                    Files.Add(new FileModel()
                    {
                        Name=f.Name,
                        Extension=f.Extension,
                        Length=f.Length
                    });
               }                
            }
        }

    }



    public class FileModel
    {
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Length { get; set; }
    }
}
