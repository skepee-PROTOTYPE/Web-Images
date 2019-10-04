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

        public FileUploadController(IHostingEnvironment _hostingEnv, IHttpContextAccessor _httpContextAccessor)
        {
            hostingEnv = _hostingEnv;
            httpContextAccessor = _httpContextAccessor;
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
                    var path = Path.Combine(hostingEnv.WebRootPath, "imagefolder", typeFolder, formFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            return View("Index");
        }
    }
}