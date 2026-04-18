namespace hotel.DTOs
{
    public class InsertMessagesDTO
    {
        public int id_enviado_por { get; set; }
        public string texto_mensagem { get; set; }
        public int id_recebido_por { get; set; }
    }
}
