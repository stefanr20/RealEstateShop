using RealEstate.BLL.Models;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Mappers
{
    public static class AddressMapper
    {
        public static AddressViewModel MapToViewModel (this Address address)
        {
            return new AddressViewModel
            {
                Id = address.Id,
                City = address.City,
                Street = address.Street,
                Country = address.Country,
            };
        }
    }
}
