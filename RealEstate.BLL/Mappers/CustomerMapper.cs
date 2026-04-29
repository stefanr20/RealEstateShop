using RealEstate.BLL.Models;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerViewModel MapToViewModel (this Customer customer)
        {
            return new CustomerViewModel
           { 
                Id = customer.Id,
                FirstName= customer.FirstName,
                LastName= customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
            
            };
        }
    }
}
