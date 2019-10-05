using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.Context
{
    public class IjpFile
    {
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public float LengthKB { get; set; }
        public float LengthMB { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public string File { get; set; }
    }
}
