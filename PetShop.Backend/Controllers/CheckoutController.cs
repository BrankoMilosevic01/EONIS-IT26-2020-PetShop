using Microsoft.AspNetCore.Mvc;
using PetShop.Backend.Data;
using PetShop.Backend.Models;
using Stripe;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TvojProjekat.Models;

namespace PetShop.Backend.Controllers
{
    public class CheckoutRequest
    {
        public decimal UkupnaCena { get; set; }
        public string OpisProizvoda { get; set; } = string.Empty;
        public string EmailKupca { get; set; } = string.Empty;
        public string AdresaKupca { get; set; } = string.Empty;
    }

    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly PetShopContext _context;

        public CheckoutController(PetShopContext context)
        {
            _context = context;
        }

        [HttpPost("create-checkout-session")]
        public ActionResult Create([FromBody] CheckoutRequest req)
        {
            var domain = "http://localhost:4200";

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                CustomerEmail = string.IsNullOrEmpty(req.EmailKupca) ? null : req.EmailKupca,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(req.UkupnaCena * 100),
                            Currency = "rsd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Pet Shop Korpa",
                            },
                        },
                        Quantity = 1,
                    },
                },
                Metadata = new Dictionary<string, string>
                {
                    { "Kupljeno", req.OpisProizvoda },
                    { "Adresa", req.AdresaKupca }
                },
                Mode = "payment",
                SuccessUrl = domain + "?success=true",
                CancelUrl = domain + "?canceled=true",
            };

            var service = new SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            return Ok(new { url = session.Url });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ParseEvent(json);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    if (session != null)
                    {
                        string adresaZaSlanje = (session.Metadata != null && session.Metadata.ContainsKey("Adresa")) ? session.Metadata["Adresa"] : "Nepoznata adresa";
                        string kupljeniArtikli = (session.Metadata != null && session.Metadata.ContainsKey("Kupljeno")) ? session.Metadata["Kupljeno"] : "Nepoznati proizvodi";

                        var novaPorudzbina = new Porudzbina
                        {
                            EmailKupca = session.CustomerDetails?.Email ?? session.CustomerEmail ?? "nepoznato@kupac.com",
                            UkupnaCena = (session.AmountTotal ?? 0) / 100m,
                            KupljeniProizvodi = $"Artikli: {kupljeniArtikli} | Adresa: {adresaZaSlanje}",
                            Status = "Plaćeno"
                        };

                        _context.Porudzbine.Add(novaPorudzbina);
                        await _context.SaveChangesAsync();
                    }
                }

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}