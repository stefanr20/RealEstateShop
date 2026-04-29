using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        public ICollection<Property>? Properties { get; set; } // Na edna Address moze da ima poveke Properties

    }
}
