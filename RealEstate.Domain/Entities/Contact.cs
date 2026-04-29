using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public ICollection<Customer>? Customers { get; set; } //eden kontakt moze da pripagja na povekje kupuvaci
    }
}
