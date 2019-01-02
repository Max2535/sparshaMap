using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SparShaMap.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FullName { get; set; }
    }
}
