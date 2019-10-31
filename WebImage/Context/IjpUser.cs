using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebImage.Context
{
    public class IjpUser
    {
        public string Pwd { get; set; }
        public string Username { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public bool Active  { get; set; }
    }
}
