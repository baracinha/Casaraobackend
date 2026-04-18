using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using hotel.Data;
using hotel.DTOs;
using hotel.Models;
using BCrypt.Net;
using System;
using Microsoft.AspNetCore.Identity;
using hotel.Jwt;

namespace hotel.Controllers
{
    [Route("/")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _context;

        private readonly TokenProvider _tokenProvider;

        public AuthenticationController(AppDbContext context, TokenProvider tokenProvider)
        {
            _context = context;
            _tokenProvider = tokenProvider;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO userDTO)
        {
            if (await _context.utilizadores.AnyAsync(u => u.nome == userDTO.nome))
            {
                return BadRequest("name already exists.");
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
            else
            {
                var token = _tokenProvider.GenerateToken(user);
                return Ok(new
                {
                    token = token,
                    id = user.id,
                    nome = user.nome,
                    email = user.email,
                    telefone = user.telefone
                });
            }
        }
    }
}
