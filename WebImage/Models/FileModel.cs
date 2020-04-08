using Microsoft.WindowsAzure.Storage;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebImage.Context;

namespace WebImage.Models
{
    public class FileModel : IjpFile
    {
        public string Url { get; set; }
        public string UrlResized { get; set; }
        public string UrlThumb { get; set; }
        public byte[]? Content { get; set; }


        public async Task<byte[]> GetContent(string imagename, string userId)
        {
            var list = await AzureStorage.ListBlobsFlatListingAsync(userId, null);
            var elem = list.Single(x => x.Key == imagename);
            var value = elem.Value;
            return value;
        }

        public async Task<byte[]> DownloadImage(string blobReferenceKey, string container)
        {
            var byteImage = await AzureStorage.DownloadFileFromBlob(blobReferenceKey, container);
            return byteImage;
        }

        public async Task<string> GetUrl(string containerName, string blobName)
        {
            var url = await AzureStorage.BlobUrlAsync(containerName, blobName);
            return url;
        }


        public void Info(string myfile, string resized, string thumb, string userid)
        {
            FileInfo f = new FileInfo(myfile);

            var sizekb = Math.Round(((double)f.Length) / 1024, 1);

            using Bitmap b = new Bitmap(myfile);

            this.Name = f.Name;
            this.Title = f.Name;
            this.RawFormat = f.Extension;
            this.LengthKB = sizekb;
            this.IsPrivate = true;
            this.IsLandscape = ((b.Width > b.Height) ? true : false);
            this.PixelFormat = b.PixelFormat.ToString();
            this.Width = b.Width;
            this.Height = b.Height;
            this.HorizontalResolution = b.HorizontalResolution;
            this.VerticalResolution = b.VerticalResolution;
            this.ThumbFileName = thumb;
            this.ResizedFileName = resized;
            this.UserId = userid;
            this.CategoryId = 1;
        }
    }
}
