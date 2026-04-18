using hotel.Data;
using hotel.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using hotel.DTOs;
using hotel.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace hotel.Controllers
{
    [Route("/")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public MessageController(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpGet("ListUsers")]
        public async Task<IActionResult> ListUsers([FromQuery] ListContactsDTO listContactsDTO)
        {
           /* string cacheKey = "ListUsers_" + listContactsDTO.id;

            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                Console.WriteLine("Data retrieved from cache.");
                return Ok(JsonConvert.DeserializeObject<List<ListContactsDTO>>(cachedData));
            }
            Console.WriteLine("Data retrieved from database.");*/
            var user = await _context.utilizadores.Where(u => u.id != listContactsDTO.id).ToListAsync();
/*
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });*/

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

        [HttpGet("ListMessages")]
        public async Task<IActionResult> ListMessages([FromQuery] ListMessagesDTO listMessagesDTO)
        {
            var messages = await _context.mensagens
                .Where(m => (m.id_enviado_por == listMessagesDTO.id_enviado_por && m.id_recebido_por == listMessagesDTO.id_recebido_por) ||
                            (m.id_enviado_por == listMessagesDTO.id_recebido_por && m.id_recebido_por == listMessagesDTO.id_enviado_por))
                .ToListAsync();
            return Ok(messages);
        }
    }
}
