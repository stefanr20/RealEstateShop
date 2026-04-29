using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstate.BLL.Mappers;
using RealEstate.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace RealEstateShop.WebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertyController : Controller
    {
        private readonly IPropertyService _propertyService;
        private readonly IAddressService _addressService;
        private readonly ICustomerService _customerService;

        public PropertyController(IPropertyService propertyService, IAddressService addressService, ICustomerService customerService)
        {
            _propertyService = propertyService;
            _addressService = addressService;
            _customerService = customerService;
        }

        // GET /api/property — returns 200 OK
        [HttpGet]
        public IActionResult Index()
        {
            var properties = _propertyService.GetAll();
            if (properties == null)
                throw new KeyNotFoundException("No properties found.");
            var viewModels = properties.Select(p => p.MapToViewModel()).ToList();
            return Ok(viewModels); // 200 OK
        }

        // GET /api/property/{id} — returns 200 OK or 404 Not Found
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Property ID must be a positive number.");
            var property = _propertyService.GetById(id);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {id} was not found.");
            return Ok(property.MapToViewModel()); // 200 OK
        }

        // GET /api/property/search — returns 200 OK
        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string query = "",
            [FromQuery] string type = "all",
            [FromQuery] string minPrice = "",
            [FromQuery] string maxPrice = "",
            [FromQuery] int? bedrooms = null)
        {
            var properties = _propertyService.Search(query, type, minPrice, maxPrice, bedrooms);
            var viewModels = properties.Select(p => p.MapToViewModel()).ToList();
            return Ok(viewModels); // 200 OK
        }

        // GET /api/property/paged — returns 200 OK or 400 Bad Request
        [HttpGet("paged")]
        public IActionResult GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 9)
        {
            if (page <= 0)
                throw new ArgumentException("Page number must be greater than 0.");
            if (pageSize <= 0 || pageSize > 100)
                throw new ArgumentException("Page size must be between 1 and 100.");

            var properties = _propertyService.GetAll().ToList();
            var total = properties.Count;
            var items = properties
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => p.MapToViewModel())
                .ToList();

            return Ok(new // 200 OK
            {
                items = items,
                total = total,
                page = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling((double)total / pageSize)
            });
        }

        // POST /api/property — returns 201 Created or 400 Bad Request
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PropertyCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Property data is required.");

            var address = new Address
            {
                City = dto.Address?.City,
                Street = dto.Address?.Street,
                Country = dto.Address?.Country
            };
            await _addressService.Create(address);

            var property = new Property
            {
                Title = dto.Title,
                Description = dto.Description,
                Price = dto.Price,
                Photo = dto.Photo,
                Bedrooms = dto.Bedrooms,
                Bathrooms = dto.Bathrooms,
                Area = dto.Area,
                Type = dto.Type,
                IsFeatured = dto.IsFeatured,
                Floor = dto.Floor,
                TotalFloors = dto.TotalFloors,
                YearBuilt = dto.YearBuilt,
                ParkingSpots = dto.ParkingSpots,
                HeatingType = dto.HeatingType,
                HasGarage = dto.HasGarage,
                HasElevator = dto.HasElevator,
                HasBalcony = dto.HasBalcony,
                HasPool = dto.HasPool,
                HasInternet = dto.HasInternet,
                IsFurnished = dto.IsFurnished,
                HasAirConditioning = dto.HasAirConditioning,
                HasSecurity = dto.HasSecurity,
                AddressId = address.Id
            };
            await _propertyService.Create(property);
            return CreatedAtAction(nameof(Details), new { id = property.Id }, property); // 201 Created
        }

        // PUT /api/property — returns 200 OK or 404 Not Found
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] PropertyCreateDto dto)
        {
            if (dto == null)
                throw new ArgumentException("Property data is required.");

            var property = _propertyService.GetById(dto.Id);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {dto.Id} was not found.");

            property.Title = dto.Title;
            property.Description = dto.Description;
            property.Price = dto.Price;
            property.Photo = dto.Photo;
            property.Bedrooms = dto.Bedrooms;
            property.Bathrooms = dto.Bathrooms;
            property.Area = dto.Area;
            property.Type = dto.Type;
            property.IsFeatured = dto.IsFeatured;
            property.Floor = dto.Floor;
            property.TotalFloors = dto.TotalFloors;
            property.YearBuilt = dto.YearBuilt;
            property.ParkingSpots = dto.ParkingSpots;
            property.HeatingType = dto.HeatingType;
            property.HasGarage = dto.HasGarage;
            property.HasElevator = dto.HasElevator;
            property.HasBalcony = dto.HasBalcony;
            property.HasPool = dto.HasPool;
            property.HasInternet = dto.HasInternet;
            property.IsFurnished = dto.IsFurnished;
            property.HasAirConditioning = dto.HasAirConditioning;
            property.HasSecurity = dto.HasSecurity;

            if (property.Address != null)
            {
                property.Address.City = dto.Address?.City;
                property.Address.Street = dto.Address?.Street;
                property.Address.Country = dto.Address?.Country;
                await _addressService.Update(property.Address);
            }

            await _propertyService.Update(property);
            return Ok(property.MapToViewModel()); // 200 OK
        }

        // DELETE /api/property/{id} — returns 200 OK or 404 Not Found
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Property ID must be a positive number.");

            var property = _propertyService.GetById(id);
            if (property == null)
                throw new KeyNotFoundException($"Property with ID {id} was not found.");

            await _propertyService.Delete(id);
            return Ok(new { message = $"Property with ID {id} was successfully deleted." }); // 200 OK
        }
    }

    public class AddressDto
    {
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }
    }

    public class PropertyCreateDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; }

        public string Photo { get; set; }

        [Range(1, 20)]
        public int Bedrooms { get; set; }

        [Range(1, 20)]
        public int Bathrooms { get; set; }

        [Range(1, 10000)]
        public int Area { get; set; }

        public string Type { get; set; }
        public bool IsFeatured { get; set; }

        public int? Floor { get; set; }
        public int? TotalFloors { get; set; }
        public int? YearBuilt { get; set; }
        public int? ParkingSpots { get; set; }
        public string HeatingType { get; set; }

        public bool HasGarage { get; set; }
        public bool HasElevator { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasPool { get; set; }
        public bool HasInternet { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasSecurity { get; set; }

        public AddressDto Address { get; set; }
    }
}