using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebImage.Context
{
    [Table("GalleryFile")]
    public class IjpGalleryFile
    {
        [Key]
        public int GalleryFileId { get; set; }
        public int GalleryId { get; set; }
        public int FileId { get; set; }
        public string Description { get; set; }
    }
}