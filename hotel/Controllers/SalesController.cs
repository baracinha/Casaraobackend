using hotel.Data;
using hotel.Services;
using hotel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using hotel.DTOs;

namespace hotel.Controllers
{
    [Route("/")]
    [ApiController]
    public class SalesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly SalesServices _salesServices;

        public SalesController(AppDbContext context, IDistributedCache cache, SalesServices salesServices)
        {
            _context = context;
            _cache = cache;
            _salesServices = salesServices;
        }

        [HttpPost("InsertProperty")]
        public async Task<IActionResult> InsertProperty([FromBody] InsertPropertyDTO propertyDTO)
        {
            await _salesServices.InsertProperty(propertyDTO);
            return Ok(propertyDTO);
        }
    }
}
