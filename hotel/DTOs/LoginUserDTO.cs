using System;
using System.Collections.Generic;
using System.Linq;

namespace hotel.DTOs
{
    public class LoginUserDTO
    {
        public string? token { get; set; }
        public int id { get; set; }
        public string username { get; set; }
        public string? email { get; set; }
        public string? telefone { get; set; }
        public string password_hash { get; set; }
        public DateTime tempo_inscrito { get; set; }
    }
}
