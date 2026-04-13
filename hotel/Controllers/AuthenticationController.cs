using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotel.Data;
using hotel.DTOs;
using hotel.Models;
using BCrypt.Net;
using System;

namespace hotel.Controllers
{
    [Route("/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthenticationController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO userDTO)
        {
            if (await _context.utilizador.AnyAsync(u => u.email == userDTO.email))
            {
                return BadRequest("Email already exists.");
            }
            var user = new utilizador
            {
                username = userDTO.username,
                email = userDTO.email,
                telefone = userDTO.telefone,
                password = BCrypt.Net.BCrypt.HashPassword(userDTO.password)
            };
            _context.utilizador.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "User registered successfully",
                id = user.id,
            });
        }
    }
}
