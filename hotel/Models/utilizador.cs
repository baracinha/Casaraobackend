using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
    [Table("utilizadores")]
    public class utilizadores
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public int telefone { get; set; }
        public string password_hash { get; set; }
        public string cargo { get; set; }
        public string bio { get; set; }
        public string imagem_perfil { get; set; }
        public string cidade { get; set; }
        public string created_at { get; set; }

    }
}
