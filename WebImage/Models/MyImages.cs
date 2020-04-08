using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebImage.DBContext;

namespace WebImage.Models
{
    public class MyImages
    {
        private readonly IjpContext ijpContext;
        public List<FileModel> Images { get; set; }

        public MyImages(IjpContext _ijpContext, string userId)
        {
            ijpContext = _ijpContext;
            Images = new List<FileModel>();
            LoadMyImages(userId);
        }

        public void AddImageFromFile(string file, string resized, string thumb, string userid)
        {
            var myfile = new FileModel();
            myfile.Info(file, resized, thumb, userid);

            ijpContext.File.Add(myfile);
            ijpContext.SaveChanges();
        }


        public void UpdatePrivateStatus(int fileId, bool isPrivate)
        {
            ijpContext.File.FirstOrDefault(x => x.FileId == fileId).IsPrivate = isPrivate;
            ijpContext.SaveChanges();
        }


        private void LoadMyImages(string userId)
        {
            foreach (var myimg in ijpContext.File.Where(x => x.UserId == userId))
            {
                Images.Add(new FileModel()
                {
                    FileId = myimg.FileId,
                    CategoryId = myimg.CategoryId,
                    RawFormat = myimg.RawFormat,
                    IsPrivate = myimg.IsPrivate,
                    IsLandscape = myimg.IsLandscape,
                    LengthKB = myimg.LengthKB,
                    Name = myimg.Name,
                    Title = myimg.Title,
                    Width = myimg.Width,
                    Height = myimg.Height,
                    HorizontalResolution = myimg.HorizontalResolution,
                    VerticalResolution = myimg.VerticalResolution,
                    PixelFormat = myimg.PixelFormat,
                    Url = AzureStorage.BlobUrlAsync(userId, myimg.Name).GetAwaiter().GetResult().ToString(),
                    UrlResized = AzureStorage.BlobUrlAsync(userId, myimg.ResizedFileName).GetAwaiter().ToString(),
                    UrlThumb = AzureStorage.BlobUrlAsync(userId, myimg.ThumbFileName).GetAwaiter().GetResult().ToString(),
                    ThumbFileName = myimg.ThumbFileName,
                    ResizedFileName = myimg.ResizedFileName,
                    Content = AzureStorage.DownloadFileFromBlob(myimg.Name,userId).GetAwaiter().GetResult(),
                    UserId = userId
                });
            }
        }

        public FileModel GetImage(int imageId)
        {
            return this.Images.FirstOrDefault(x => x.FileId == imageId);
        } 
    }
}
