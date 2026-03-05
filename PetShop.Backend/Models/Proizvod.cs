namespace PetShop.Backend.Models
{
    public class Proizvod
    {
        public int Id { get; set; }

        public string Naziv { get; set; } = string.Empty;
        public string Opis { get; set; } = string.Empty;
        public decimal Cena { get; set; }
        public int KolicinaNaStanju { get; set; }
        public string Slika { get; set; } = string.Empty;

        public int KategorijaId { get; set; }
        public string Potkategorija { get; set; } = string.Empty;
    }
}