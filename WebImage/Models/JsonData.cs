using System.Collections.Generic;

namespace WebImage.Models
{
    public class JsonGallery
    {
        public JsonData Data { get; set; }
        public JsonStats Stat { get; set; }

        public JsonGallery()
        {
            Data = new JsonData();
        }
    }


    public class JsonData
    {
        public List<MyJson> MyJson { get; set; }
        public string Profile { get; set; }

        public JsonData()
        {
            MyJson = new List<MyJson>();
        }
    }


    public class MyJson
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Format { get; set; }
        public double Length { get; set; }
        public string IsLandscape { get; set; }
        public string PixelFormat { get; set; }
        public string Size { get; set; }
        public string Resolution { get; set; }
        public string Url { get; set; }
        public string UrlThumb { get; set; }
        public string UrlResized { get; set; }
        public byte[] Content { get; set; }
    }


    public class JsonStats
    {
        public int Count { get; set; }
        public double TotalLengthKb { get; set; }
        public double ElapsedTime { get; set; }
    }
}
