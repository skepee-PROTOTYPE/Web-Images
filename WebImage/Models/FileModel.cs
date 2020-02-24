using WebImage.Context;

namespace WebImage.Models
{
    public class FileModel : IjpFile
    {
        public string Path { get; set; }
        public string Url { get; set; }
        public string Thumb { get; set; }
        public byte[]? Content { get; set; }
    }
}
