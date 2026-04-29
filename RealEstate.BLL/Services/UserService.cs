using RealEstate.BLL.Interfaces;
using RealEstate.Data.UnitOfWork;
using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.Users.GetAll();
        }

        public User GetById(int id)
        {
            return _unitOfWork.Users.GetById(id);
        }

        public async Task Create(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            _unitOfWork.Users.Insert(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Update(User user)
        {
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();
        }

        public async Task Delete(int id)
        {
            _unitOfWork.Users.Delete(id);
            await _unitOfWork.CompleteAsync();
        }

        public User Login(string username, string password)
        {
            var user = _unitOfWork.Users.GetAll()
                .FirstOrDefault(u => u.Username == username);

            if (user == null) return null;

            bool isValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isValid ? user : null;
        }

        public bool UsernameExists(string username)
        {
            return _unitOfWork.Users.GetAll()
                .Any(u => u.Username == username);
        }

        public async Task UpdateRole(int id, string role)
        {
            var user = _unitOfWork.Users.GetById(id);
            if (user != null)
            {
                user.Role = role;
                _unitOfWork.Users.Update(user);
                await _unitOfWork.CompleteAsync();
            }
        }
    }
}