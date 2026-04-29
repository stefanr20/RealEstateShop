using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var contacts = _contactService.GetAll();
            return Ok(contacts);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var contact = _contactService.GetById(id);
            if (contact == null)
                return NotFound();
            return Ok(contact);
        }

        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            _contactService.Create(contact);
            return Ok(contact);
        }

        [HttpPut]
        public IActionResult Update(Contact contact)
        {
            _contactService.Update(contact);
            return Ok(contact);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _contactService.Delete(id);
            return Ok();
        }
    }
}