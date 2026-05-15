export interface Property {
  id: number;
  title: string;
  description: string;
  price: string;
  photo: string;
  photos?: string[];
  bedrooms: number;
  bathrooms: number;
  area: number;
  type: string;
  isFeatured?: boolean;
  address: Address;

  // Details
  floor?: number | null;
  totalFloors?: number | null;
  yearBuilt?: number | null;
  parkingSpots?: number | null;
  heatingType?: string | null;

  // Amenities
  hasGarage?: boolean;
  hasElevator?: boolean;
  hasBalcony?: boolean;
  hasPool?: boolean;
  hasInternet?: boolean;
  isFurnished?: boolean;
  hasAirConditioning?: boolean;
  hasSecurity?: boolean;
}

export interface Address {
  id: number;
  city: string;
  street: string;
  country: string;
}