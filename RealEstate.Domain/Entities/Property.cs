using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Entities
{
    public class Property
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters")] //so ova se pravime bezbednosta da bide pogolema odnosno da ne moze nekoj da si vnesuva gluposti nego se da bide validno i ako ne e dobro vrakja error
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public string Price { get; set; }

        public string Photo { get; set; }

        [Range(1, 20, ErrorMessage = "Bedrooms must be between 1 and 20")]
        public int Bedrooms { get; set; }

        [Range(1, 20, ErrorMessage = "Bathrooms must be between 1 and 20")]
        public int Bathrooms { get; set; }

        [Range(1, 10000, ErrorMessage = "Area must be between 1 and 10000")]
        public int Area { get; set; }

        public string Type { get; set; }
        public bool IsFeatured { get; set; }

        //detail fields
        public int? Floor { get; set; }
        public int? TotalFloors { get; set; }
        public int? YearBuilt { get; set; }
        public int? ParkingSpots { get; set; }
        public string HeatingType { get; set; }

        // features fields
        public bool HasGarage { get; set; }
        public bool HasElevator { get; set; }
        public bool HasBalcony { get; set; }
        public bool HasPool { get; set; }
        public bool HasInternet { get; set; }
        public bool IsFurnished { get; set; }
        public bool HasAirConditioning { get; set; }
        public bool HasSecurity { get; set; }

        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
    }
}