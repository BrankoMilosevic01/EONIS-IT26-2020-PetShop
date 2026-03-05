using Microsoft.AspNetCore.Mvc;

namespace PetShop.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategorijeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetKategorije()
        {
            var kategorije = new[]
            {
                new { id = 1, name = "Kuce", subcategories = new[] { "Hrana", "Odeća", "Higijena" } },
                new { id = 2, name = "Mace", subcategories = new[] { "Hrana", "Posip", "Igračke" } },
                new { id = 3, name = "Akvaristika", subcategories = new[] { "Ribice", "Kornjače", "Akvarijumi" } }
            };

            return Ok(kategorije);
        }
    }
}