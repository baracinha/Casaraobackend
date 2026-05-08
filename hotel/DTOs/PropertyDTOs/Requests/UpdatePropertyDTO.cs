namespace hotel.DTOs.PropertyDTOs.Requests
{
    public class UpdatePropertyDTO
    {
        public int id { get; set; }
        public int id_utilizador { get; set; }
        public string titulo { get; set; }
        public string descricao { get; set; }
        public decimal preco { get; set; }
        public string cidade { get; set; }
        public List<string> imagens { get; set; }
        public List<string> caracteristicas { get; set; }
    }
}
