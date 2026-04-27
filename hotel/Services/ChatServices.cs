using hotel.Data;
using hotel.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace hotel.Services
{
    public class ChatServices
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public ChatServices(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<ListContactsDTO>> GetContacts(ListContactsDTO listContactsDTO)
        {
            string cacheKey = $"ListUsers_{listContactsDTO.id}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                Console.WriteLine("Data retrieved from cache. " + cachedData);
                return JsonConvert.DeserializeObject<List<ListContactsDTO>>(cachedData);
            }
            Console.WriteLine("Data retrieved from database.");
            var user = await _context.utilizadores.Where(u => u.id != listContactsDTO.id).Select(u => new ListContactsDTO
            {
                id = u.id,
                nome = u.nome
            }).ToListAsync();

            if (user.Any())
            {
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }
            return user;
        }

        public async Task<BasicListDTO> GetBasicList(BasicListDTO basicListDTO)
        {
            var user = await _context.utilizadores.FirstOrDefaultAsync(u => u.nome == basicListDTO.nome);
            if (user != null)
            {
                return new BasicListDTO
                {
                    id = user.id,
                    nome = user.nome
                };
            }
            return null;
        }

        public async Task<List<ListMessagesDTO>> GetMessages(ListMessagesDTO listMessagesDTO)
        {
            string cacheKey = $"ListMessages_{listMessagesDTO.id_enviado_por}_{listMessagesDTO.id_recebido_por}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedData))
            {
                Console.WriteLine("Data retrieved from cache. " + cachedData);
                return JsonConvert.DeserializeObject<List<ListMessagesDTO>>(cachedData);
            }
            var messages = await _context.mensagens
                .Where(m => (m.id_enviado_por == listMessagesDTO.id_enviado_por && m.id_recebido_por == listMessagesDTO.id_recebido_por) ||
                            (m.id_enviado_por == listMessagesDTO.id_recebido_por && m.id_recebido_por == listMessagesDTO.id_enviado_por))
                .Select(m => new ListMessagesDTO
                {
                    id = m.id,
                    id_enviado_por = m.id_enviado_por,
                    texto_mensagem = m.texto_mensagem,
                    id_recebido_por = m.id_recebido_por
                })
                .ToListAsync();
                if (messages.Any())
                {
                    await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(messages), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
            }
            return messages;
        }
    }
}
