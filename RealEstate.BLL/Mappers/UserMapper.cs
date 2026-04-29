using RealEstate.Domain.Entities;
using RealEstate.BLL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Mappers
{
    public static class UserMapper
    {
        public static UserViewModel MapToViewModel (this User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };
        }
    }
}
