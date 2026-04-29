using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Interfaces
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Contact GetById(int id);
        Task Create(Contact contact);
        Task Update(Contact contact);
        Task Delete(int id);
    }

}
