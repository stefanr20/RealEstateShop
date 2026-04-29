using Microsoft.AspNetCore.Mvc;
using RealEstate.Data.Context;
using RealEstate.Domain.Entities;
using System.Threading.Tasks;

namespace RealEstateShop.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubscriberController : Controller
    {
        private readonly RealEstateDbContext _context;

        public SubscriberController(RealEstateDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeDto dto)
        {
            if (string.IsNullOrEmpty(dto.Email))
                return BadRequest(new { message = "Email is required" });

            var exists = _context.Subscribers.Any(s => s.Email == dto.Email);
            if (exists)
                return BadRequest(new { message = "Already subscribed" });

            var subscriber = new Subscriber { Email = dto.Email };
            _context.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Subscribed successfully" });
        }
    }

    public class SubscribeDto
    {
        public string Email { get; set; }
    }
}
