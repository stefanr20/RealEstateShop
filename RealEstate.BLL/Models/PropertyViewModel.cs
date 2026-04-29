using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstate.BLL.Models
{
    public class PropertyViewModel
    {
        public PropertyViewModel() { }

        //Property 
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Photo { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Area { get; set; }
        public string Type { get; set; }
        public bool IsFeatured { get; set; }

        // Details
        public int? Floor { get; set; }
        public int? TotalFloors { get; set; }
        public int? YearBuilt { get; set; }
        public int? ParkingSpots { get; set; }
        public string HeatingType { get; set; }

        // Features
        public bool HasGarage { get; set; }
        public bool HasElevator { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasPool { get; set; }
        public bool HasInternet { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasSecurity { get; set; }

        //Address
        public string City { get; set; }
        public string Street { get; set; }
        public string Country { get; set; }

        //Customer
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}