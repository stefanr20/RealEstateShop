using RealEstate.BLL.Interfaces;
using RealEstate.Data.UnitOfWork;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Services
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Contact> GetAll()
        {
            return _unitOfWork.Contacts.GetAll();
        }

        public Contact GetById(int id)
        {
            return _unitOfWork.Contacts.GetById(id);
        }

        public async Task Create(Contact contact)
        {
            _unitOfWork.Contacts.Insert(contact);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(Contact contact)
        {
            _unitOfWork.Contacts.Update(contact);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Contacts.Delete(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}