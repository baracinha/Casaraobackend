using System;
using System.Collections.Generic;
using System.Linq;

namespace hotel.DTOs
{
    public class RegisterUserDTO
    {
        public string nome { get; set; }
        public string email { get; set; }
        public int telefone { get; set; } = 0;
        public string password_hash { get; set; }
        public string cargo { get; set; }
        public string bio { get; set; } = "";
        public string imagem_perfil { get; set; } = null;
        public string cidade { get; set; }
        public string created_at { get; set; }
    }
}
