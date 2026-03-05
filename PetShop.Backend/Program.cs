using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetShop.Backend.Data;
using PetShop.Backend.Models;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PetShopContext>(opcije =>
    opcije.UseSqlite("Data Source=petshop.db"));

builder.Services.AddIdentity<Korisnik, IdentityRole>(opcije =>
{
    opcije.Password.RequireDigit = false;
    opcije.Password.RequiredLength = 6;
    opcije.Password.RequireNonAlphanumeric = false;
    opcije.Password.RequireUppercase = false;
    opcije.Password.RequireLowercase = false;
})
.AddEntityFrameworkStores<PetShopContext>()
.AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

StripeConfiguration.ApiKey = "sk_test_51T67u6BOCLmmyjYLeHYk71RaHEa4sXDIMq2MDczhoEHWh3bPLsC7sLrtwzWpsKAYtroVzB6Gek2228EDTo78pDBP003GuyRzQv";

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();