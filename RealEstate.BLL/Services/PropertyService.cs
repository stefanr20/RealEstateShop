using RealEstate.BLL.Interfaces;
using RealEstate.Data.UnitOfWork;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RealEstate.Data.Context;

namespace RealEstate.BLL.Services
{
    public class PropertyService : IPropertyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RealEstateDbContext _context;

        public PropertyService(IUnitOfWork unitOfWork, RealEstateDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public IEnumerable<Property> GetAll()
        {
            return _context.Properties.Include(p => p.Address).Include(p => p.Customer).ToList();
        }

        public Property GetById(int id)
        {
            return _context.Properties.Include(p => p.Address).Include(p => p.Customer).FirstOrDefault(p => p.Id == id);
        }

        public async Task Create(Property property)
        {
            _unitOfWork.Properties.Insert(property);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(Property property)
        {
            _unitOfWork.Properties.Update(property);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Properties.Delete(id);
            await _unitOfWork.CompleteAsync();
        }

        public IEnumerable<Property> Search(string query, string type, string minPrice, string maxPrice, int? bedrooms)
        {
            var results = _context.Properties.Include(p => p.Address).Include(p => p.Customer).ToList();

            if (!string.IsNullOrEmpty(query))
            {
                query = query.ToLower();
                results = results.Where(p =>
                    p.Title.ToLower().Contains(query) ||
                    (p.Address != null && p.Address.City.ToLower().Contains(query)) ||
                    (p.Type != null && p.Type.ToLower().Contains(query))
                ).ToList();
            }

            if (!string.IsNullOrEmpty(type) && type != "all")
            {
                results = results.Where(p => p.Type == type).ToList();
            }

            if (!string.IsNullOrEmpty(minPrice))
            {
                var min = int.Parse(minPrice);
                results = results.Where(p =>
                    int.Parse(p.Price.Replace("€", "").Replace(",", "").Trim()) >= min
                ).ToList();
            }

            if (!string.IsNullOrEmpty(maxPrice))
            {
                var max = int.Parse(maxPrice);
                results = results.Where(p =>
                    int.Parse(p.Price.Replace("€", "").Replace(",", "").Trim()) <= max
                ).ToList();
            }

            if (bedrooms.HasValue)
            {
                results = results.Where(p => p.Bedrooms >= bedrooms.Value).ToList();
            }

            return results;
        }
    }
}