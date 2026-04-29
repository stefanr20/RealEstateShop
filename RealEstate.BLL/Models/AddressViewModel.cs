using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Models
{
    public class AddressViewModel
    {
        public AddressViewModel() 
        { 
        
        }
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

    }

}
