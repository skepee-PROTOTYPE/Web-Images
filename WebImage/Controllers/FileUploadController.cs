using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebImage.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IHostingEnvironment hostingEnv;
        private readonly IjpContext ijpContext;
        private static string host;

        public FileUploadController(IHostingEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor, IjpContext _ijpContext)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            var request = httpContextAccessor.HttpContext.Request;
            host = request.Host.ToString();

        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost("FileUpload")]
        public async Task<IActionResult> Upload(List<IFormFile> files, bool isPrivate)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var filePath = Path.Combine(hostingEnv.WebRootPath, "imagefolder", formFile.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        formFile.CopyTo(stream);
                    }

                    var x = new Context.IjpFile
                    {
                        Category = "",
                        Extension = Path.GetExtension(formFile.FileName),
                        LengthKB = formFile.Length.KB(),
                        LengthMB = formFile.Length.MB(),
                        Name = formFile.Name.CleanName(),
                        Title = formFile.Name.CleanName(),
                        Content = Helper.byteFile(filePath),
                        Url = host + "/images/" + formFile.FileName,
                        IsPrivate = isPrivate
                    };

                    ijpContext.File.Add(x) ;
                    ijpContext.SaveChanges();

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }
            }

            return View("Index");
        }
    }
}