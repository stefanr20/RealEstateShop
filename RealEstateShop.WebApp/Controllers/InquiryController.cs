using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.Domain.Entities;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RealEstateShop.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InquiryController : Controller
    {
        private readonly IInquiryService _inquiryService;

        public InquiryController(IInquiryService inquiryService)
        {
            _inquiryService = inquiryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Inquiry inquiry)
        {
            await _inquiryService.Create(inquiry);
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var inquiries = _inquiryService.GetAll();
            return Ok(inquiries);
        }

        [Authorize]
        [HttpGet("my")]
        public IActionResult GetMyInquiries()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email)) return Unauthorized();
            var inquiries = _inquiryService.GetAllWithProperties()
                .Where(i => i.Email == email)
                .OrderByDescending(i => i.CreatedAt)
                .Select(i => new
                {
                    id = i.Id,
                    name = i.Name,
                    email = i.Email,
                    phone = i.Phone,
                    message = i.Message,
                    propertyId = i.PropertyId,
                    propertyTitle = i.Property != null ? i.Property.Title : null,
                    createdAt = i.CreatedAt,
                    adminReply = i.AdminReply,
                    repliedAt = i.RepliedAt
                });
            return Ok(inquiries);
        }
        [Authorize(Roles = "admin")]
        [HttpPut("{id}/reply")]
        public async Task<IActionResult> Reply(int id, [FromBody] ReplyDto dto)
        {
            await _inquiryService.Reply(id, dto.Reply);
            return Ok();
        }
        public class ReplyDto
        {
            public string Reply { get; set; }
        }
    }
}