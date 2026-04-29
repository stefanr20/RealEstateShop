using RealEstate.Data.Context;
using RealEstate.Data.Repositories;
using RealEstate.Domain.Entities;

namespace RealEstate.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RealEstateDbContext _context;
        private GenericRepository<Property> _propertyRepository;
        private GenericRepository<Customer> _customerRepository;
        private GenericRepository<Address> _addressRepository;
        private GenericRepository<Contact> _contactRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<Inquiry> _inquiryRepository;

        public UnitOfWork(RealEstateDbContext context)
        {
            _context = context;
        }

        public IGenericRepository<Property> Properties =>
            _propertyRepository ??= new GenericRepository<Property>(_context);
        public IGenericRepository<Customer> Customers =>
            _customerRepository ??= new GenericRepository<Customer>(_context);
        public IGenericRepository<Address> Addresses =>
            _addressRepository ??= new GenericRepository<Address>(_context);
        public IGenericRepository<Contact> Contacts =>
            _contactRepository ??= new GenericRepository<Contact>(_context);
        public IGenericRepository<User> Users =>
            _userRepository ??= new GenericRepository<User>(_context);
        public IGenericRepository<Inquiry> Inquiries =>
            _inquiryRepository ??= new GenericRepository<Inquiry>(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}