using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebImage.Models;

namespace WebImage
{
    public static class Helper
    {
        public static string Decode(string selectedImages)
        {
            string decodedString = "";
            if (!string.IsNullOrEmpty(selectedImages))
            {
                byte[] data = Convert.FromBase64String(selectedImages);
                decodedString = Encoding.UTF8.GetString(data);
            }

            return decodedString;
        }

        public static string Encode(string selectedFiles)
        {
            byte[] byt = Encoding.ASCII.GetBytes(selectedFiles);
            string selection = Convert.ToBase64String(byt);

            return selection;
        }


        public static string DecodeImage(byte[] file)
        {
            var base64 = Convert.ToBase64String(file);
            var imgSrc = String.Format("data:image/gif;base64,{0}", base64);

            return imgSrc;
        }


        public static string SaveResizedFile(Bitmap b,string file, int width, int height)
        {
            Bitmap resized = new Bitmap(b, new Size(width, height));
            string filename = Path.Combine(Path.GetFileNameWithoutExtension(file) + "_" + width.ToString() + "_" + height.ToString() + Path.GetExtension(file));
            string thumbName = Path.Combine(Path.GetDirectoryName(file), filename);

            if (!File.Exists(thumbName))
                resized.Save(thumbName);

            return filename;
        }
    }
}
