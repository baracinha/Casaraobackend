using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
    [Table("utilizador")]
    public class utilizador
    {
        public int id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public int telefone { get; set; }
        public string password { get; set; }
    }
}
