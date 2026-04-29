using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var addresses = _addressService.GetAll();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var address = _addressService.GetById(id);
            if (address == null)
                return NotFound();
            return Ok(address);
        }

        [HttpPost]
        public IActionResult Create(Address address)
        {
            _addressService.Create(address);
            return Ok(address);
        }

        [HttpPut]
        public IActionResult Update(Address address)
        {
            _addressService.Update(address);
            return Ok(address);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _addressService.Delete(id);
            return Ok();
        }
    }
}