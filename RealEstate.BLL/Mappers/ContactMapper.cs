using RealEstate.BLL.Models;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Mappers
{
    public static class ContactMapper
    {
        public static ContactViewModel MapToViewModel (this Contact contact)
        {
            return new ContactViewModel
            {
                Id = contact.Id,
                Email = contact.Email,
                Phone = contact.Phone,

            };
        }
    }
}
