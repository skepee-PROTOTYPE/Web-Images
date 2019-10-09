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
        public async Task<IActionResult> Upload(List<IFormFile> files, string typeFolder)
        {
            long size = files.Sum(f => f.Length);

            var filePaths = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                   // var f= System.IO.File.(formFile)

                    

                    var x = new Context.IjpFile
                    {
                        Category = "",
                        Extension = Path.GetExtension(formFile.FileName),
                        LengthKB = formFile.Length.KB(),
                        LengthMB = formFile.Length.MB(),
                        Name = formFile.Name.CleanName(),
                        Title = formFile.Name.CleanName(),
                        Content = Helper.byteFile(formFile.FileName),
                        Url = host + "/images/" + formFile.FileName
                        //IsPrivate = isPrivate
                    };


                    ijpContext.File.Add(x) ;

                    ijpContext.SaveChanges();

                    //var path = Path.Combine(hostingEnv.WebRootPath, "imagefolder", typeFolder, formFile.FileName);

                    //using (var stream = new FileStream(path, FileMode.Create))
                    //{
                    //    await formFile.CopyToAsync(stream);
                    //}
                }
            }

            return View("Index");
        }
    }
}