using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Models;

namespace WebImage
{
    public class JsonPreviewModel : PageModel
    {
     //   public MyGalleries myGalleries{ get; set; }

        public void OnGet()
        {
     //       myGalleries = new MyGalleries();
    //        var id = myGalleries.Gallery.FirstOrDefault(x => x.IsSelected).Gallery.GalleryId;
      //      myGalleries.Gallery.FirstOrDefault(x => x.IsSelected).JsonGallery = myGalleries.GetFileInfoJson(id);
        }
    }
}