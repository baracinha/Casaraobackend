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
                id_utilizador = propertyDTO.id_utilizador,
                titulo = propertyDTO.titulo,
                descricao = propertyDTO.descricao,
                preco = propertyDTO.preco,
                tipo_propriedade = propertyDTO.tipo_propriedade,
                endereco = propertyDTO.endereco,
                cidade = propertyDTO.cidade,
                quartos = propertyDTO.quartos,
                casa_banho = propertyDTO.casa_banho,
                area_m2 = propertyDTO.area_m2,
                tipo_negocio = propertyDTO.tipo_negocio,
            };
            _context.propriedades.Add(property);
            await _context.SaveChangesAsync();

            var pasta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "properties", property.id.ToString());
            if (!Directory.Exists(pasta))
            {
                Directory.CreateDirectory(pasta);
            }

            int ordem = 0;

            foreach (var imagem in propertyDTO.imagens)
            {
                var base64 = imagem.Contains(",") ? imagem.Split(',')[1] : imagem;
                var imagemBytes = Convert.FromBase64String(base64);
                var nomeImagem = $"imagem_{ordem}.jpg";
                var caminhoImagem = Path.Combine(pasta, nomeImagem);
                await File.WriteAllBytesAsync(caminhoImagem, imagemBytes);
                var imagemPropriedade = new imagens_propriedades
                {
                    id_propriedade = property.id,
                    image_url = $"/images/properties/{property.id}/{nomeImagem}"
                };
                _context.imagens_propriedades.Add(imagemPropriedade);
                ordem++;
            }

            var caracteristicas = new List<propriedades_caracteristicas>();
            foreach (var idCaracteristica in propertyDTO.caracteristicas)
            {
                var caracteristicaPropriedade = new propriedades_caracteristicas
                {
                    id_propriedade = property.id,
                    id_caracteristica = idCaracteristica
                };
                caracteristicas.Add(caracteristicaPropriedade);
            }
            _context.propriedades_caracteristicas.AddRange(caracteristicas);
            await _context.SaveChangesAsync();
            return propertyDTO;
        }
    }
}
