using hotel.Data;
using hotel.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using hotel.DTOs;
using hotel.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using hotel.Services;

namespace hotel.Controllers
{
    [Route("/")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ChatServices _chatServices;

        public MessageController(AppDbContext context, IDistributedCache cache, ChatServices chatServices)
        {
            _context = context;
            _cache = cache;
            _chatServices = chatServices;
        }

        [HttpGet("ListUsers")]
        public async Task<IActionResult> ListUsers([FromQuery] ListContactsDTO listContactsDTO)
        {
            var user = await _chatServices.GetContacts(listContactsDTO);
            return Ok(user);
        }

        [HttpGet("BasicList")]
        public async Task<IActionResult> BasicList([FromQuery] BasicListDTO basicListDTO)
        {
            var user = await _chatServices.GetBasicList(basicListDTO);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound("User not found.");
            }
        }

        [HttpPost("InsertMessages")]
        public async Task<IActionResult> InsertMessages([FromBody] InsertMessagesDTO insertMessagesDTO)
        {
            await _chatServices.SendMessage(insertMessagesDTO);
            return Ok(new { message = "Message sent successfully" });
        }

        [HttpGet("ListMessages")]
        public async Task<IActionResult> ListMessages([FromQuery] ListMessagesDTO listMessagesDTO)
        {
            var messages = await _chatServices.GetMessages(listMessagesDTO);
            return Ok(messages);
        }
    }
}
