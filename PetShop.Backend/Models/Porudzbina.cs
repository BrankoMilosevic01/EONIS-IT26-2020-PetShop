using System;

namespace TvojProjekat.Models
{
    public class Porudzbina
    {
        public int Id { get; set; }

        public string? EmailKupca { get; set; }
        public string? KupljeniProizvodi { get; set; }

        public decimal UkupnaCena { get; set; }
        public DateTime DatumPorudzbine { get; set; } = DateTime.Now;

        public string? Status { get; set; } = "Na čekanju";
    }
}