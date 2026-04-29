using RealEstate.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RealEstate.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<User> GetAll();
        User GetById(int id);
        Task Create(User user);
        Task Update(User user);
        Task Delete(int id);
        User Login(string username, string password);
        bool UsernameExists(string username);
        Task UpdateRole(int id, string role);
    }
}