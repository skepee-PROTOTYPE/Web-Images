using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebImage.Context;
using WebImage.Models;

namespace WebImage.Pages
{
    public class JsonModel : PageModel
    {
        public List<FileModel> MyFiles { get; set; }

        public string ApiGetUrl { get; set; }
        public string TypeSelected { get; set; }
        public List<IjpCategory> Category { get; set; }

        public void OnGet()
        {

        }
    }
}
