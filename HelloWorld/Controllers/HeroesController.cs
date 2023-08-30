using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HelloWorld.Controllers;

[ApiController]
[Route("[controller]")]
public class HeroesController : ControllerBase
{
    private readonly List<Hero> heroes = new();

    public HeroesController() { }

    private JsonSerializerOptions getJsonSerializerOptions() {
        return new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    [HttpGet(Name = "List Heroes")]
    public async Task<IActionResult> IndexAsync() {
        try {
            HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient
                .GetAsync("https://fbc05bfa-5b53-4305-a609-59790e7ab497.mock.pstmn.io/heroes"); // Replace with the actual API endpoint

            response.EnsureSuccessStatusCode();

            List<Hero>? heroes = JsonSerializer.Deserialize<List<Hero>>(
                await response.Content.ReadAsStringAsync(),
                getJsonSerializerOptions()
            );

            return Ok(heroes);
        }
        catch (HttpRequestException ex) {
            return StatusCode(500, $"Error: {ex.Message}");
        }
    }
}

