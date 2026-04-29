using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }    
        public string Email { get; set; }
        public string Phone {  get; set; }
        public int? ContactId { get; set; }
        public Contact? Contact { get; set; }

        public ICollection<Property>? Properties { get; set; } //eden Customer moze da ima poveke Properties

    }
}
