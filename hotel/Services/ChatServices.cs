using hotel.Data;
using hotel.DTOs.MessageDTOs.Requests;
using hotel.DTOs.MessageDTOs.Responses;
using hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Nest;
using Newtonsoft.Json;

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

        public async Task<List<ListContactsResponseDTO>> GetContacts(ListContactsDTO listContactsDTO)
        {
            string cacheKey = $"ListUsers_{listContactsDTO.id}";
            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cachedData))
            {
                Console.WriteLine("Data retrieved from cache. " + cachedData);
                return JsonConvert.DeserializeObject<List<ListContactsResponseDTO>>(cachedData);
            }
            Console.WriteLine("Data retrieved from database.");
            var user = await _context.conversas.Where(u => u.id_enviante == listContactsDTO.id).Select(u => new ListContactsResponseDTO
            { 
                id = u.id_receptor,
                id_propriedade = u.id_propriedade,
                nome_propriedade = _context.propriedades.Where(x => x.id == u.id_propriedade).Select(x => x.titulo).FirstOrDefault(),
                nome = _context.utilizadores.Where(x => x.id == u.id_receptor).Select(x => x.nome).FirstOrDefault()
            }).ToListAsync(); 

            if (user.Any())
            {
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });

                return user;
            }else {                 var user2 = await _context.conversas.Where(u => u.id_receptor == listContactsDTO.id).Select(u => new ListContactsResponseDTO
                {
                    id = u.id_enviante,
                    id_propriedade = u.id_propriedade,
                    nome_propriedade = _context.propriedades.Where(x => x.id == u.id_propriedade).Select(x => x.titulo).FirstOrDefault(),
                    nome = _context.utilizadores.Where(x => x.id == u.id_enviante).Select(x => x.nome).FirstOrDefault()
                }).ToListAsync();
                if (user2.Any())
                {
                    await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(user2), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    });
                    return user2;
                }

            }
            return new List<ListContactsResponseDTO>();
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

        public async Task SendMessage(InsertMessagesDTO insertMessagesDTO)
        {

            var message = new mensagens
            {
                id_enviado_por = insertMessagesDTO.id_enviado_por,
                texto_mensagem = insertMessagesDTO.texto_mensagem,
                id_recebido_por = insertMessagesDTO.id_recebido_por
            };
            _context.mensagens.Add(message);
            await _context.SaveChangesAsync();
            await ClearCache($"ListMessages_{insertMessagesDTO.id_enviado_por}_{insertMessagesDTO.id_recebido_por}");
        }

        public async Task<GenerateChatDTO> GenerateChat(GenerateChatDTO generateChatDTO)
        {
            var existingChat = await _context.conversas.FirstOrDefaultAsync(c =>
                (c.id_enviante == generateChatDTO.id_enviante && c.id_receptor == generateChatDTO.id_receptor && c.id_propriedade == generateChatDTO.id_propriedade) ||
                (c.id_enviante == generateChatDTO.id_receptor && c.id_receptor == generateChatDTO.id_enviante && c.id_propriedade == generateChatDTO.id_propriedade));
            if (existingChat != null)
            {
                return new GenerateChatDTO
                {
                    id_enviante = existingChat.id_enviante,
                    id_receptor = existingChat.id_receptor,
                    id_propriedade = existingChat.id_propriedade
                };
            }
            var newChat = new conversas
            {
                id_enviante = generateChatDTO.id_enviante,
                id_receptor = generateChatDTO.id_receptor,
                id_propriedade = generateChatDTO.id_propriedade
            };
            _context.conversas.Add(newChat);
            await _context.SaveChangesAsync();
            await ClearCache($"ListUsers_{generateChatDTO.id_enviante}");
            await ClearCache($"ListUsers_{generateChatDTO.id_receptor}");
            return new GenerateChatDTO
            {
                id_enviante = newChat.id_enviante,
                id_receptor = newChat.id_receptor,
                id_propriedade = newChat.id_propriedade
            };
        }

        public async Task ClearCache(string cacheKey)
        {
            await _cache.RemoveAsync(cacheKey);
        }
        public async Task ClearAllCache()
        {
            var cacheKeys = new List<string> { /* List of all cache keys */ };
            foreach (var key in cacheKeys)
            {
                await _cache.RemoveAsync(key);
            }
        }
    }
}
