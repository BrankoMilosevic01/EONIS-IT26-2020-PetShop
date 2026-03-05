using Microsoft.AspNetCore.Mvc;
using PetShop.Backend.Data;
using PetShop.Backend.Models;
using System.Linq;

namespace PetShop.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProizvodiController : ControllerBase
    {
        private readonly PetShopContext _context;

        public ProizvodiController(PetShopContext context)
        {
            _context = context;

            if (!_context.Proizvodi.Any())
            {
                var proizvodi = new[]
                {
                    new Proizvod { KategorijaId = 1, Potkategorija = "Hrana", Naziv = "Royal Canin hrana za pse 10kg", Cena = 5998, KolicinaNaStanju = 5, Slika = "/royal_canin.webp" },
                    new Proizvod { KategorijaId = 1, Potkategorija = "Hrana", Naziv = "Obrok u alu posudici", Cena = 139, KolicinaNaStanju = 20, Slika = "/alu_posuda.jpg" },
                    new Proizvod { KategorijaId = 1, Potkategorija = "Hrana", Naziv = "Nagradice za pse", Cena = 509, KolicinaNaStanju = 23, Slika = "/nagradice.png" },
                    new Proizvod { KategorijaId = 1, Potkategorija = "Higijena", Naziv = "Ogrlica protiv buva", Cena = 4590, KolicinaNaStanju = 10, Slika = "/ogrlica_buve.jpg" },
                    new Proizvod { KategorijaId = 1, Potkategorija = "Higijena", Naziv = "Vitamini za pse", Cena = 2767, KolicinaNaStanju = 15, Slika = "/vitamini.avif" },
                    new Proizvod { KategorijaId = 1, Potkategorija = "Odeća", Naziv = "Džemper za pse", Cena = 2390, KolicinaNaStanju = 13, Slika = "/dzemper.webp" },
                    new Proizvod { KategorijaId = 1, Potkategorija = "Odeća", Naziv = "Kabanica za pse", Cena = 3465, KolicinaNaStanju = 8, Slika = "/kabanica.jpg" },

                    new Proizvod { KategorijaId = 2, Potkategorija = "Hrana", Naziv = "Felix", Cena = 2410, KolicinaNaStanju = 9, Slika = "/Felix_mace.jpg" },
                    new Proizvod { KategorijaId = 2, Potkategorija = "Hrana", Naziv = "Whiskas", Cena = 1450, KolicinaNaStanju = 11, Slika = "/whiskas.jpg" },
                    new Proizvod { KategorijaId = 2, Potkategorija = "Igračke", Naziv = "Miš za igranje", Cena = 1280, KolicinaNaStanju = 6, Slika = "/mis.webp" },
                    new Proizvod { KategorijaId = 2, Potkategorija = "Posip", Naziv = "Posip za mace", Cena = 890, KolicinaNaStanju = 34, Slika = "/posip.jpg" },

                    new Proizvod { KategorijaId = 3, Potkategorija = "Ribice", Naziv = "Hrana za ribice", Cena = 1230, KolicinaNaStanju = 26, Slika = "/hrana_ribice.webp" },
                    new Proizvod { KategorijaId = 3, Potkategorija = "Kornjače", Naziv = "Hrana za kornjače", Cena = 1300, KolicinaNaStanju = 4, Slika = "/hrana_kornjace.avif" },
                    new Proizvod { KategorijaId = 3, Potkategorija = "Akvarijumi", Naziv = "Pumpa za vazduh", Cena = 5600, KolicinaNaStanju = 5, Slika = "/pumpa_vazduh.jpg" },
                    new Proizvod { KategorijaId = 3, Potkategorija = "Akvarijumi", Naziv = "Pumpa za vodu", Cena = 5500, KolicinaNaStanju = 1, Slika = "/pumpa_voda.jpg" }
                };

                _context.Proizvodi.AddRange(proizvodi);
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IActionResult GetProizvodi()
        {
            return Ok(_context.Proizvodi.ToList());
        }

        [HttpGet("{id}")]
        public IActionResult GetProizvod(int id)
        {
            var proizvod = _context.Proizvodi.Find(id);
            if (proizvod == null) return NotFound("Proizvod nije pronađen.");
            return Ok(proizvod);
        }

        [HttpPost]
        public IActionResult DodajProizvod([FromBody] Proizvod noviProizvod)
        {
            _context.Proizvodi.Add(noviProizvod);
            _context.SaveChanges();
            return Ok(noviProizvod);
        }

        [HttpPut("{id}")]
        public IActionResult IzmeniProizvod(int id, [FromBody] Proizvod izmenjen)
        {
            var postojeci = _context.Proizvodi.Find(id);
            if (postojeci == null) return NotFound("Proizvod nije pronađen.");

            postojeci.Naziv = izmenjen.Naziv;
            postojeci.Opis = izmenjen.Opis;
            postojeci.Cena = izmenjen.Cena;
            postojeci.KolicinaNaStanju = izmenjen.KolicinaNaStanju;
            postojeci.Slika = izmenjen.Slika;
            postojeci.KategorijaId = izmenjen.KategorijaId;
            postojeci.Potkategorija = izmenjen.Potkategorija;

            _context.SaveChanges();
            return Ok(postojeci);
        }

        [HttpDelete("{id}")]
        public IActionResult ObrisiProizvod(int id)
        {
            var proizvod = _context.Proizvodi.Find(id);
            if (proizvod == null) return NotFound("Proizvod nije pronađen.");

            _context.Proizvodi.Remove(proizvod);
            _context.SaveChanges();
            return Ok(new { poruka = "Proizvod je uspešno obrisan." });
        }
    }
}