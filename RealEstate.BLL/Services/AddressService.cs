using RealEstate.BLL.Interfaces;
using RealEstate.Data.UnitOfWork;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Services
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddressService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Address> GetAll()
        {
            return _unitOfWork.Addresses.GetAll();
        }

        public Address GetById(int id)
        {
            return _unitOfWork.Addresses.GetById(id);
        }

        public async Task Create(Address address)
        {
            _unitOfWork.Addresses.Insert(address);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(Address address)
        {
            _unitOfWork.Addresses.Update(address);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Addresses.Delete(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}