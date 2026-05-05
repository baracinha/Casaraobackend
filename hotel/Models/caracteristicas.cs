
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
    [Table("caracteristicas")]
    public class caracteristicas
    {
        public int id { get; set; }
        public string nome { get; set; }
    }
}
