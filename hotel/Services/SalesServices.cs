using hotel.Data;
using hotel.DTOs;
using hotel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace hotel.Services
{
    public class SalesServices
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;

        public SalesServices(AppDbContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<InsertPropertyDTO> InsertProperty(InsertPropertyDTO propertyDTO)
        {
            var property = new Models.propriedades
            {
                id = propertyDTO.id,
                id_utilizador = propertyDTO.id_utilizador,
                titulo = propertyDTO.titulo,
                descricao = propertyDTO.descricao,
                preco = propertyDTO.preco,
                tipo_propriedade = propertyDTO.tipo_propriedade,
                endereco = propertyDTO.endereco,
                cidade = propertyDTO.cidade,
                quartos = propertyDTO.quartos,
                casa_banho = propertyDTO.casa_banho,
                area_m2 = propertyDTO.area_m2
            };
            _context.propriedades.Add(property);
            await _context.SaveChangesAsync();
            return propertyDTO;
        }
    }
}
