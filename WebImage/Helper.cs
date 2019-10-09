using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebImage
{
    public static class Helper
    {
        public static byte[] byteFile(string varFilePath)
        {
            byte[] file;
            using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }
            return file;
        }


        public static double KB(this long length)
        {
            var sizekb = Math.Round(((double)length) / 1024, 1);
            return sizekb;
        }

        public static double MB(this long length)
        {
            var sizeMb = Math.Round(length.KB() / 1024, 2);
            return sizeMb;
        }

        public static string CleanName(this string name)
        {
            return name.Replace("@", "").Replace(".", "");
        }

        public static void DownloadFile(byte[] file, string name)
        {
            using (var fs = new FileStream(@"D:\images\" + name, FileMode.Create, FileAccess.Write))
                fs.Write(file, 0, file.Length);
        }

        public static string Decode(string selectedImages)
        {
            byte[] data = Convert.FromBase64String(selectedImages);
            string decodedString = Encoding.UTF8.GetString(data);
            return decodedString;
        }

        public static string Encode(string selectedFiles)
        {
            byte[] byt = System.Text.Encoding.ASCII.GetBytes(selectedFiles);
            string selection = Convert.ToBase64String(byt);

            return selection;
        }


        public static string DecodeImage(byte[] file)
        {
            var base64 = Convert.ToBase64String(file);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            return imgSrc;
        }

    }
}
