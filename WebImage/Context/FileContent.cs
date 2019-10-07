using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.Context
{
    [Table("FileContent")]
    public class FileContent
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Extension { get; set; }
        public double LengthKB { get; set; }
        public double LengthMB { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public byte[] Content { get; set; }
    }
}
