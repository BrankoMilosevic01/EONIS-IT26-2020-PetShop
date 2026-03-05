using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Backend.Data;
using PetShop.Backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TvojProjekat.Models;

namespace PetShop.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PorudzbineController : ControllerBase
    {
        private readonly PetShopContext _context;

        public PorudzbineController(PetShopContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Porudzbina>>> GetPorudzbine()
        {
            return await _context.Porudzbine
                                 .OrderByDescending(p => p.DatumPorudzbine)
                                 .ToListAsync();
        }

        [HttpGet("moje/{email}")]
        public async Task<ActionResult<IEnumerable<Porudzbina>>> GetMojePorudzbine(string email)
        {
            var mojePorudzbine = await _context.Porudzbine
                                               .Where(p => p.EmailKupca == email)
                                               .OrderByDescending(p => p.DatumPorudzbine)
                                               .ToListAsync();

            if (mojePorudzbine == null)
            {
                return NotFound();
            }

            return Ok(mojePorudzbine);
        }
    }
}