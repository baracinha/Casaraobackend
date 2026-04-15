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
            if (await _context.utilizadores.AnyAsync(u => u.email == userDTO.email))
            {
                return BadRequest("Email already exists.");
            }
            var user = new utilizadores
            {
                nome = userDTO.nome,
                email = userDTO.email,
                telefone = userDTO.telefone,
                password_hash = BCrypt.Net.BCrypt.HashPassword(userDTO.password_hash),
                cargo = userDTO.cargo,
                bio = userDTO.bio,
                imagem_perfil = userDTO.imagem_perfil,
                cidade = userDTO.cidade,
            };
            _context.utilizadores.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "User registered successfully",
                id = user.id,
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginUserDTO loginDTO)
        {
            var user = await _context.utilizadores.FirstOrDefaultAsync(u => u.nome == loginDTO.username);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.password_hash, user.password_hash))
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok(new
            {
                message = "Login successful",
                id = user.id,
                nome = user.nome,
                email = user.email,
                cargo = user.cargo,
                bio = user.bio,
                imagem_perfil = user.imagem_perfil,
                cidade = user.cidade,
            });
        }
    }
}
