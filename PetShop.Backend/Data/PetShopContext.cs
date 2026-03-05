using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PetShop.Backend.Models;
using TvojProjekat.Models;

namespace PetShop.Backend.Data
{
    public class PetShopContext : IdentityDbContext<Korisnik>
    {
        public PetShopContext(DbContextOptions<PetShopContext> options) : base(options)
        {
        }

        public DbSet<Proizvod> Proizvodi { get; set; }

        public DbSet<Porudzbina> Porudzbine { get; set; }
    }
}