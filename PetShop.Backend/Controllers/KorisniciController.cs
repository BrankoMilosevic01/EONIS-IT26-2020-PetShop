using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PetShop.Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PetShop.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KorisniciController : ControllerBase
    {
        private readonly UserManager<Korisnik> _userManager;

        public KorisniciController(UserManager<Korisnik> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("registracija")]
        public async Task<IActionResult> Registracija([FromBody] RegistracijaDto podaci)
        {
            if (string.IsNullOrWhiteSpace(podaci.Email) || !new EmailAddressAttribute().IsValid(podaci.Email))
            {
                return BadRequest("Neispravan format email adrese.");
            }

            var postojeci = await _userManager.FindByEmailAsync(podaci.Email);
            if (postojeci != null) return BadRequest("Korisnik sa ovim emailom već postoji!");

            var noviKorisnik = new Korisnik
            {
                UserName = podaci.Email,
                Email = podaci.Email,
                ImePrezime = podaci.ImePrezime,
                Ulica = podaci.Ulica,
                KucniBroj = podaci.KucniBroj,
                BrojStana = podaci.BrojStana,
                Mesto = podaci.Mesto,
                PostanskiBroj = podaci.PostanskiBroj,
                Uloga = podaci.Email == "admin@petshop.com" ? "ADMIN" : "USER"
            };

            var rezultat = await _userManager.CreateAsync(noviKorisnik, podaci.Lozinka);

            if (rezultat.Succeeded)
            {
                return Ok(new { poruka = "Uspešna registracija!" });
            }

            return BadRequest(rezultat.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto podaci)
        {
            var korisnik = await _userManager.FindByEmailAsync(podaci.Email);
            if (korisnik == null) return Unauthorized("Pogrešan email ili lozinka.");

            var ispravnaLozinka = await _userManager.CheckPasswordAsync(korisnik, podaci.Lozinka);
            if (!ispravnaLozinka) return Unauthorized("Pogrešan email ili lozinka.");

            return Ok(new
            {
                id = korisnik.Id,
                email = korisnik.Email,
                ime = korisnik.ImePrezime,
                uloga = korisnik.Uloga,
                punaAdresa = $"{korisnik.Ulica} {korisnik.KucniBroj}{(string.IsNullOrEmpty(korisnik.BrojStana) ? "" : "/" + korisnik.BrojStana)}, {korisnik.PostanskiBroj} {korisnik.Mesto}"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKorisnici()
        {
            var korisnici = await _userManager.Users.Select(k => new {
                k.Id,
                k.ImePrezime,
                k.Email,
                k.Uloga,
                Adresa = $"{k.Ulica} {k.KucniBroj}, {k.Mesto}"
            }).ToListAsync();

            return Ok(korisnici);
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> GetProfil(string email)
        {
            var korisnik = await _userManager.FindByEmailAsync(email);
            if (korisnik == null) return NotFound("Korisnik nije pronađen.");
            return Ok(korisnik);
        }

        [HttpPut("{email}")]
        public async Task<IActionResult> AzurirajProfil(string email, [FromBody] IzmenaProfilaDto podaci)
        {
            var korisnik = await _userManager.FindByEmailAsync(email);
            if (korisnik == null) return NotFound("Korisnik nije pronađen.");

            korisnik.ImePrezime = podaci.ImePrezime;
            korisnik.Ulica = podaci.Ulica;
            korisnik.KucniBroj = podaci.KucniBroj;
            korisnik.BrojStana = podaci.BrojStana;
            korisnik.Mesto = podaci.Mesto;
            korisnik.PostanskiBroj = podaci.PostanskiBroj;

            var rezultat = await _userManager.UpdateAsync(korisnik);
            if (rezultat.Succeeded) return Ok(new { poruka = "Profil uspješno ažuriran!" });

            return BadRequest("Greška pri ažuriranju profila.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> ObrisiKorisnika(string id)
        {
            var korisnik = await _userManager.FindByIdAsync(id);
            if (korisnik == null) return NotFound("Korisnik nije pronađen.");

            if (korisnik.Uloga == "ADMIN") return BadRequest("Ne možete obrisati administratora!");

            var rezultat = await _userManager.DeleteAsync(korisnik);
            if (rezultat.Succeeded) return Ok(new { poruka = "Korisnik uspješno obrisan." });

            return BadRequest("Greška pri brisanju korisnika.");
        }
    }

    public class RegistracijaDto
    {
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
        public string ImePrezime { get; set; } = string.Empty;
        public string Ulica { get; set; } = string.Empty;
        public string KucniBroj { get; set; } = string.Empty;
        public string BrojStana { get; set; } = string.Empty;
        public string Mesto { get; set; } = string.Empty;
        public string PostanskiBroj { get; set; } = string.Empty;
    }

    public class LoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Lozinka { get; set; } = string.Empty;
    }

    public class IzmenaProfilaDto
    {
        public string ImePrezime { get; set; } = string.Empty;
        public string Ulica { get; set; } = string.Empty;
        public string KucniBroj { get; set; } = string.Empty;
        public string BrojStana { get; set; } = string.Empty;
        public string Mesto { get; set; } = string.Empty;
        public string PostanskiBroj { get; set; } = string.Empty;
    }
}