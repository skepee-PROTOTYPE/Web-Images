using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebImage.Context;
using WebImage.DBContext;

namespace WebImage.Models
{
    public class MyImages
    {
        private readonly IjpContext ijpContext;
        public List<FileModel> Images { get; set; }

        public MyImages(IjpContext _ijpContext)
        {
            ijpContext = _ijpContext;
            Images = new List<FileModel>();
        }

        public void LoadMyImages(string userId, string Host, string wwwrootpath)
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
                    Url = Host + "/images/" + myimg.Name,
                    Path = Path.Combine(".", "imagefolder", myimg.Name),
                    Thumb = Path.Combine(".", "imagefolder", "Thumbs", Path.GetFileNameWithoutExtension(myimg.Name) + "_thumb" + Path.GetExtension(myimg.Name)),
                    Content = Helper.byteFile(Path.Combine(wwwrootpath, "imagefolder", myimg.Name)),
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
