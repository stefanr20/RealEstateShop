using RealEstate.Data.Repositories;
using RealEstate.Domain.Entities;

namespace RealEstate.Data.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Property> Properties { get; }
        IGenericRepository<Customer> Customers { get; }
        IGenericRepository<Address> Addresses { get; }
        IGenericRepository<Contact> Contacts { get; }
        IGenericRepository<User> Users { get; }
        IGenericRepository<Inquiry> Inquiries { get; }

        Task <int> CompleteAsync();
    }

}
