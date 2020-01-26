using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebImage.Context
{
    [Table("FileContent")]
    public class IjpFile
    {
        [Key]
        public int FileId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string RawFormat { get; set; }
        public double? LengthKB { get; set; }
        public int CategoryId { get; set; }
        public bool IsPrivate { get; set; }
        public bool? IsLandscape { get; set; }
        public string PixelFormat { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public double? HorizontalResolution { get; set; }
        public double? VerticalResolution { get; set; }
    }
}
