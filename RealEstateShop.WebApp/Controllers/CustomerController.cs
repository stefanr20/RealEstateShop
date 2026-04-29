using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Mappers;
using RealEstate.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstateShop.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var customers = _customerService.GetAll();
            var viewModels = customers.Select(c => c.MapToViewModel()).ToList();
            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            var customer = _customerService.GetById(id);
            if (customer == null)
                return NotFound();
            return Ok(customer.MapToViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            await _customerService.Create(customer);
            return Ok(customer);
        }

        [HttpPut]
        public async Task<IActionResult> Edit(Customer customer)
        {
            await _customerService.Update(customer);
            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _customerService.Delete(id);
            return Ok();
        }
    }
}