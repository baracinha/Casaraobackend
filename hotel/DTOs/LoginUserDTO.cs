using System;
using System.Collections.Generic;
using System.Linq;

namespace hotel.DTOs
{
    public class LoginUserDTO
    {
        public string username { get; set; }
        public string password_hash { get; set; }
    }
}
