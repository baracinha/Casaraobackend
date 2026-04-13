using System;
using System.Collections.Generic;
using System.Linq;

namespace hotel.DTOs
{
    public class RegisterUserDTO
    {
        public string username { get; set; }
        public string email { get; set; }
        public int telefone { get; set; }
        public string password { get; set; }
    }
}
