using RealEstate.BLL.Interfaces;
using RealEstate.Data.UnitOfWork;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Customer> GetAll()
        {
            return _unitOfWork.Customers.GetAll();
        }

        public Customer GetById(int id)
        {
            return _unitOfWork.Customers.GetById(id);
        }

        public async Task Create(Customer customer)
        {
            _unitOfWork.Customers.Insert(customer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(Customer customer)
        {
            _unitOfWork.Customers.Update(customer);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Customers.Delete(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}