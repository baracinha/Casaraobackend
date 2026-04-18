using hotel.Data;
using hotel.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using hotel.DTOs;
using hotel.Models;

namespace hotel.Controllers
{
    [Route("/")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MessageController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("ListUsers")]
        public async Task<IActionResult> ListUsers([FromQuery] ListContactsDTO listContactsDTO)
        {
            var user = await _context.utilizadores.Where(u => u.id != listContactsDTO.id).ToListAsync();

            return Ok(user);
        }

        [HttpGet("BasicList")]
        public async Task<IActionResult> BasicList([FromQuery] BasicListDTO basicListDTO)
        {
            var user = await _context.utilizadores.FirstOrDefaultAsync(u => u.nome == basicListDTO.nome);
            return Ok(user);
        }

        [HttpPost("InsertMessages")]
        public async Task<IActionResult> InsertMessages([FromBody] InsertMessagesDTO insertMessagesDTO)
        {
            var message = new mensagens
            {
                id_enviado_por = insertMessagesDTO.id_enviado_por,
                texto_mensagem = insertMessagesDTO.texto_mensagem,
                id_recebido_por = insertMessagesDTO.id_recebido_por
            };
            _context.mensagens.Add(message);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Message sent successfully", id = message.id });
        }
    }
}
