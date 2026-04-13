using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace hotel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class controladorteste : ControllerBase
    {
        [HttpGet]
        public string get()
        {
            return "Olá, mundo!";
        }

        [HttpGet("adoro")]
        public string getAdoro()
        {
            return "Eu adoro estas coisas";
        }



        [HttpGet("Getimposter")]
        public async Task<IActionResult> GetImposter()
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    string apiUrl = "http://localhost:4547/api/cars";

                    var response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return Ok(content);
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, $"Erro ao chamar a API: {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao chamar a API: {ex.Message}");
                }
            }
        }


    }
}
