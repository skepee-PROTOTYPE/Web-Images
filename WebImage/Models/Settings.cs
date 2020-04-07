using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.Models
{
    public static class Settings
    {
        public static string ConnectionString{get;set;}
        public static string StorageAccount { get; set; }
        public static string Secrets { get; set; }
        public static string TenantId { get; set; }
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }
    }
}
