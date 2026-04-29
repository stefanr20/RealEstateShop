using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {

        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

    }

}
