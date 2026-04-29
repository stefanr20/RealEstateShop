using RealEstate.BLL.Models;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Interfaces
{
    public interface IPropertyService
    {
        IEnumerable<Property> GetAll();
        Property GetById(int id);
        Task Create(Property property);
        Task Update(Property property);
        Task Delete(int id);
        IEnumerable<Property> Search(string query, string type, string minPrice, string maxPrice, int? bedrooms);
    }
}
