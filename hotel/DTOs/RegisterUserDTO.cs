using System;
using System.Collections.Generic;
using System.Linq;

namespace hotel.DTOs
{
    public class RegisterUserDTO
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string password_hash { get; set; }
        public string cargo { get; set; }
        public string bio { get; set; } 
        public string imagem_perfil { get; set; }
        public string cidade { get; set; }
    }
}
