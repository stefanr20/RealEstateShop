using Microsoft.AspNetCore.Mvc;
using RealEstate.BLL.Interfaces;
using RealEstateShop.WebApp.Models;
using System.Diagnostics;
using System.Linq;
using RealEstate.BLL.Mappers;

namespace RealEstateShop.WebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly IPropertyService _propertyService;

        public HomeController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        public IActionResult Index()
        {
            var properties = _propertyService.GetAll();
            var viewModels = properties.Select(p => p.MapToViewModel()).ToList();
            return View(viewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
