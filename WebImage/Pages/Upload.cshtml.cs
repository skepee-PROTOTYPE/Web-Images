using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.DBContext;
using WebImage.Models;

namespace WebImage.Pages
{
    public class UploadModel : PageModel
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private IWebHostEnvironment environment;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IjpContext ijpContext;
        public MyImages myImages { get; set; }

        public UploadModel(IHttpContextAccessor _httpContextAccessor, IWebHostEnvironment _environment, UserManager<IdentityUser> _userManager, IjpContext _ijpContext)
        {
            httpContextAccessor = _httpContextAccessor;
            ijpContext = _ijpContext;
            environment = _environment;
            userManager = _userManager;
            string userId = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Contains("nameidentifier")).Value;
            myImages = new MyImages(ijpContext, userId);
        }

        [BindProperty]
        public List<IFormFile> Upload { get; set; }
        public async Task OnPostAsync()
        {
            var user = await userManager.GetUserAsync(User);

            string mydir = Path.Combine(environment.WebRootPath, "imagefolder", user.Id);
            Directory.CreateDirectory(mydir);

            foreach (var file in Upload)
            {
                FileInfo f = new FileInfo(file.FileName);
                string originalFile = Path.Combine(mydir, file.FileName);

                using (var fileStream = new FileStream(originalFile, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                using Bitmap b = new Bitmap(originalFile);

                string thumb=Helper.SaveResizedFile(b, originalFile, 200, 200);
                string resized=Helper.SaveResizedFile(b, originalFile, b.Width/2, b.Height/2);

                await AzureStorage.AddFilesAsync(originalFile, user.Id);
                await AzureStorage.AddFilesAsync(Path.Combine(mydir, thumb), user.Id);
                await AzureStorage.AddFilesAsync(Path.Combine(mydir, resized), user.Id);

                var myImages = new MyImages(ijpContext, user.Id);
                myImages.AddImageFromFile(originalFile, resized, thumb, user.Id);

                System.IO.File.Delete(file.FileName);
            }
            Directory.Delete(Path.Combine(environment.WebRootPath, "imagefolder", user.Id),true);
        }

    }
}
