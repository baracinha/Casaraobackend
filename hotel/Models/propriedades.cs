using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace hotel.Models
{
    [Table("propriedades")]
    public class propriedades
    {
        public int id { get; set; }
        public int id_utilizador { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public decimal preco { get; set; }
        public string tipo_propriedade { get; set; }
        public string status_propriedade { get; set; } = "disponivel";
        public string endereco { get; set; }
        public string cidade { get; set; }
        public int quartos { get; set; }
        public int casa_banho { get; set; }
        public decimal area_m2 { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;

    }
}
