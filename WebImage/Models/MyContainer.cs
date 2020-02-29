using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Linq;
using WebImage.DBContext;

namespace WebImage.Models
{
    public class MyContainer
    {
        private readonly IjpContext ijpContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IWebHostEnvironment hostingEnv;
        private string Host { get; set; }

        public MyGalleries myGalleries { get; set; }
        public MyImages myImages { get; set; }

        public MyContainer(IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext, IWebHostEnvironment _hostingEnv)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;

            var request = httpContextAccessor.HttpContext.Request;
            Host = request.Host.ToString();

            myGalleries = new MyGalleries(ijpContext);
            myImages = new MyImages(ijpContext);        
        }


        public JsonData GetFileInfoJson()
        {
            JsonData myData = new JsonData();

            string hidecolumn = Helper.Decode(this.myGalleries.Gallery[0].Gallery.Columns);

            foreach (var imageGallery in this.myGalleries.Gallery[0].GalleryFile)
            {
                var image = myImages.Images.FirstOrDefault(x => x.FileId == imageGallery.FileId);

                if (image != null)
                {
                    myData.MyJson.Add(new MyJson()
                    {
                        Url = hidecolumn.Contains("url") ? "" : image.Url,
                        Name = hidecolumn.Contains("name") ? "" : image.Name,
                        Title = hidecolumn.Contains("title") ? "" : image.Title,
                        Format = hidecolumn.Contains("format") ? "" : image.RawFormat,
                        Length = hidecolumn.Contains("length") ? 0 : (double)image.LengthKB,
                        IsLandscape = (hidecolumn.Contains("landscape") ? null : image.IsLandscape.ToString()),
                        PixelFormat = hidecolumn.Contains("pixel") ? "" : image.PixelFormat,
                        Size = hidecolumn.Contains("size") ? "" : image.Width.ToString() + " X " + image.Height.ToString(),
                        Resolution = hidecolumn.Contains("resolution") ? "" : image.HorizontalResolution + " X " + image.VerticalResolution,
                        Content = hidecolumn.Contains("content") ? null : image.Content
                    });
                }
            }
            return myData;
        }


    }
}
