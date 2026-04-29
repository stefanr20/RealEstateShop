using RealEstate.BLL.Models;
using RealEstate.Domain.Entities;

namespace RealEstate.BLL.Mappers
{
    public static class PropertyMapper
    {
        public static PropertyViewModel MapToViewModel(this Property property)
        {
            return new PropertyViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Price = property.Price,
                Photo = property.Photo,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                Area = property.Area,
                Type = property.Type,
                IsFeatured = property.IsFeatured,

                Floor = property.Floor,
                TotalFloors = property.TotalFloors,
                YearBuilt = property.YearBuilt,
                ParkingSpots = property.ParkingSpots,
                HeatingType = property.HeatingType,
                HasGarage = property.HasGarage,
                HasElevator = property.HasElevator,
                HasBalcony = property.HasBalcony,
                HasPool = property.HasPool,
                HasInternet = property.HasInternet,
                IsFurnished = property.IsFurnished,
                HasAirConditioning = property.HasAirConditioning,
                HasSecurity = property.HasSecurity,

                City = property.Address?.City,
                Street = property.Address?.Street,
                Country = property.Address?.Country,
                FirstName = property.Customer?.FirstName,
                LastName = property.Customer?.LastName
            };
        }
    }
}