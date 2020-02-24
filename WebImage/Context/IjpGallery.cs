using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebImage.Context
{
    [Table("Gallery")]
    public class IjpGallery
    {
        [Key]
        public int GalleryId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Columns { get; set; }
        public string Images { get; set; }
        public bool Active { get; set; }
        public DateTime DateInsert { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}