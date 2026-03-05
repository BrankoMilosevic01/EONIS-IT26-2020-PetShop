using Microsoft.AspNetCore.Identity;

namespace PetShop.Backend.Models
{
    public class Korisnik : IdentityUser
    {
        public string ImePrezime { get; set; } = string.Empty;

        public string Ulica { get; set; } = string.Empty;
        public string KucniBroj { get; set; } = string.Empty;
        public string BrojStana { get; set; } = string.Empty;
        public string Mesto { get; set; } = string.Empty;
        public string PostanskiBroj { get; set; } = string.Empty;

        public string Uloga { get; set; } = "USER";
    }
}