using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.Domain.Entities;

namespace RealEstate.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;

        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var properties = _propertyService.GetAll();
            return Ok(properties);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var property = _propertyService.GetById(id);
            if (property == null)
                return NotFound();
            return Ok(property);
        }

        [HttpPost]
        public IActionResult Create(Property property)
        {
            _propertyService.Create(property);
            return Ok(property);
        }

        [HttpPut]
        public IActionResult Update(Property property)
        {
            _propertyService.Update(property);
            return Ok(property);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _propertyService.Delete(id);
            return Ok();
        }
    }
}