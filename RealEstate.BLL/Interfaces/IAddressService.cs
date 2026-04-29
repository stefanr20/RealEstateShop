using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Interfaces
{
    public interface IAddressService
    {
        IEnumerable<Address> GetAll();
        Address GetById(int id);
        Task Create(Address address);
        Task Update(Address address);
        Task Delete(int id);
    }
}