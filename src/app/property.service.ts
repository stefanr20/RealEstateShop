import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { fetchWithErrorHandling } from './error.interceptor';
import { Property } from './property';

@Injectable({
  providedIn: 'root'
})
export class PropertyService {
  private router = inject(Router);
  url = 'https://localhost:7112/api/property';

  mockProperties: Property[] = [
    {
      id: 1,
      title: 'Luxury Penthouse Downtown',
      description: 'A breathtaking penthouse with panoramic city views, floor-to-ceiling windows, and premium finishes throughout. Features an open-plan living area, gourmet kitchen, and a private rooftop terrace.',
      price: '€850,000',
      bedrooms: 3,
      bathrooms: 2,
      area: 180,
      type: 'apartment',
      isFeatured: true,
      photo: 'https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&q=80',
      photos: [
      'https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&q=80',
      'https://images.unsplash.com/photo-1600607687939-ce8a6c25118c?w=800&q=80',
      'https://images.unsplash.com/photo-1600566753086-00f18fb6b3ea?w=800&q=80',
      'https://images.unsplash.com/photo-1600585154526-990dced4db0d?w=800&q=80',
    ],
      address: { id: 1, city: 'Skopje', street: 'Bul. Jane Sandanski 12', country: 'Macedonia' }
    },
    {
      id: 2,
      title: 'Modern Family Villa',
      description: 'Spacious modern villa in a quiet neighborhood with a large garden, swimming pool, and double garage. Recently renovated with high-end materials and smart home features.',
      price: '€1,200,000',
      bedrooms: 5,
      bathrooms: 4,
      area: 350,
      type: 'villa',
      isFeatured: true,
      photo: 'https://images.unsplash.com/photo-1613977257363-707ba9348227?w=800&q=80',
      address: { id: 2, city: 'Ohrid', street: 'Ul. Kliment Ohridski 5', country: 'Macedonia' }
    },
    {
      id: 3,
      title: 'Cozy City Studio',
      description: 'Smart and stylish studio apartment in the heart of the city. Perfect for young professionals. Features high ceilings, exposed brick, and modern appliances.',
      price: '€95,000',
      bedrooms: 1,
      bathrooms: 1,
      area: 45,
      type: 'studio',
      photo: 'https://images.unsplash.com/photo-1522708323590-d24dbb6b0267?w=800&q=80',
      address: { id: 3, city: 'Skopje', street: 'Ul. Makedonija 3', country: 'Macedonia' }
    },
    {
      id: 4,
      title: 'Charming Old Town House',
      description: 'A beautifully restored traditional house in the historic old town. Original stone walls, wooden beams, and a private courtyard garden. Unique character and charm.',
      price: '€320,000',
      bedrooms: 3,
      bathrooms: 2,
      area: 140,
      type: 'house',
      photo: 'https://images.unsplash.com/photo-1568605114967-8130f3a36994?w=800&q=80',
      address: { id: 4, city: 'Bitola', street: 'Shirok Sokak 18', country: 'Macedonia' }
    },
    {
      id: 5,
      title: 'Lakefront Apartment',
      description: 'Stunning lakefront apartment with unobstructed water views from every room. Private balcony, underground parking, and direct access to the lake promenade.',
      price: '€480,000',
      bedrooms: 2,
      bathrooms: 2,
      area: 110,
      type: 'apartment',
      isFeatured: true,
      photo: 'https://images.unsplash.com/photo-1502672260266-1c1ef2d93688?w=800&q=80',
      address: { id: 5, city: 'Ohrid', street: 'Kej Makedonija 22', country: 'Macedonia' }
    },
    {
      id: 6,
      title: 'Executive Apartment',
      description: 'Premium executive apartment in a prestigious residential tower. Concierge service, fitness center, and rooftop pool included. Perfect for discerning buyers.',
      price: '€560,000',
      bedrooms: 3,
      bathrooms: 3,
      area: 160,
      type: 'apartment',
      photo: 'https://images.unsplash.com/photo-1560448204-e02f11c3d0e2?w=800&q=80',
      address: { id: 6, city: 'Skopje', street: 'Bul. Partizanski Odredi 45', country: 'Macedonia' }
    },
    {
      id: 7,
      title: 'Mountain View House',
      description: 'Peaceful family home with stunning mountain views. Large terrace, mature garden, and spacious interior. Ideal for those seeking tranquility without sacrificing comfort.',
      price: '€275,000',
      bedrooms: 4,
      bathrooms: 2,
      area: 200,
      type: 'house',
      photo: 'https://images.unsplash.com/photo-1512917774080-9991f1c4c750?w=800&q=80',
      address: { id: 7, city: 'Tetovo', street: 'Ul. Ilindenska 7', country: 'Macedonia' }
    },
    {
      id: 8,
      title: 'Boutique Riverside Studio',
      description: 'Architect-designed studio by the river with floor-to-ceiling glass walls. Minimalist interior, smart storage, and a private terrace overlooking the water.',
      price: '€120,000',
      bedrooms: 1,
      bathrooms: 1,
      area: 52,
      type: 'studio',
      photo: 'https://images.unsplash.com/photo-1493809842364-78817add7ffb?w=800&q=80',
      address: { id: 8, city: 'Skopje', street: 'Kej Dimitar Vlahov 9', country: 'Macedonia' }
    },
    {
      id: 9,
      title: 'Minimalist Downtown Loft',
      description: 'Industrial-chic loft in the city center with exposed concrete walls, polished floors, and floor-to-ceiling windows. Open plan living with a mezzanine bedroom and private rooftop access.',
      price: '€310,000',
      bedrooms: 2,
      bathrooms: 1,
      area: 95,
      type: 'apartment',
      photo: 'https://images.unsplash.com/photo-1536376072261-38c75010e6c9?w=800&q=80',
      address: { id: 9, city: 'Skopje', street: 'Ul. Vasil Gjorgov 14', country: 'Macedonia' }
    },
    {
      id: 10,
      title: 'Elegant Suburban Villa',
      description: 'Sophisticated villa in a prestigious suburb with manicured gardens, heated pool, and a three-car garage. The interiors feature marble flooring, custom cabinetry, and a home cinema room.',
      price: '€1,850,000',
      bedrooms: 6,
      bathrooms: 5,
      area: 520,
      type: 'villa',
      isFeatured: true,
      photo: 'https://images.unsplash.com/photo-1600585154340-be6161a56a0c?w=800&q=80',
      address: { id: 10, city: 'Skopje', street: 'Bul. Aleksandar Makedonski 3', country: 'Macedonia' }
    },
    {
      id: 11,
      title: 'Hillside Family Home',
      description: 'Spacious hillside house with breathtaking valley views. Features a large wraparound terrace, open kitchen, and a generous garden perfect for families. Recently renovated throughout.',
      price: '€395,000',
      bedrooms: 4,
      bathrooms: 3,
      area: 230,
      type: 'house',
      photo: 'https://images.unsplash.com/photo-1583608205776-bfd35f0d9f83?w=800&q=80',
      address: { id: 11, city: 'Veles', street: 'Ul. Dimitar Vlahov 8', country: 'Macedonia' }
    },
    {
      id: 12,
      title: 'Compact City Studio',
      description: 'Smartly designed studio in a sought-after central location. Fully furnished with built-in storage, a modern kitchenette, and a sunny balcony. Ideal for students or young professionals.',
      price: '€75,000',
      bedrooms: 1,
      bathrooms: 1,
      area: 38,
      type: 'studio',
      photo: 'https://images.unsplash.com/photo-1554995207-c18c203602cb?w=800&q=80',
      address: { id: 12, city: 'Skopje', street: 'Ul. Orce Nikolov 21', country: 'Macedonia' }
    },
    {
      id: 13,
      title: 'Lakeside Retreat Villa',
      description: 'Stunning lakeside villa with private dock and boat house. Panoramic lake views from every room, infinity pool, and a beautifully landscaped garden. A rare and exclusive find.',
      price: '€2,200,000',
      bedrooms: 5,
      bathrooms: 4,
      area: 400,
      type: 'villa',
      isFeatured: true,
      photo: 'https://images.unsplash.com/photo-1564013799919-ab600027ffc6?w=800&q=80',
      address: { id: 13, city: 'Ohrid', street: 'Kej Sv. Naum 1', country: 'Macedonia' }
    },
    {
      id: 14,
      title: 'Modern Corner Apartment',
      description: 'Bright corner apartment flooded with natural light. Features an open living and dining area, modern kitchen with island, two en-suite bedrooms, and a large wraparound balcony.',
      price: '€220,000',
      bedrooms: 2,
      bathrooms: 2,
      area: 98,
      type: 'apartment',
      photo: 'https://images.unsplash.com/photo-1560185007-cde436f6a4d0?w=800&q=80',
      address: { id: 14, city: 'Skopje', street: 'Bul. 8mi Septemvri 33', country: 'Macedonia' }
    },
    {
      id: 15,
      title: 'Rustic Stone Farmhouse',
      description: 'Charming stone farmhouse set on 2 hectares of land with fruit orchards and a vegetable garden. Original stone walls, wooden beams, and a large fireplace give it incredible character.',
      price: '€185,000',
      bedrooms: 3,
      bathrooms: 2,
      area: 175,
      type: 'house',
      photo: 'https://images.unsplash.com/photo-1518780664697-55e3ad937233?w=800&q=80',
      address: { id: 15, city: 'Struga', street: 'Ul. Noe Zupan 4', country: 'Macedonia' }
    },
    {
      id: 16,
      title: 'Penthouse Sky Suite',
      description: 'Exceptional penthouse occupying the entire top floor of a luxury tower. Features a 360-degree terrace, private elevator, smart home automation, and unmatched city skyline views.',
      price: '€1,450,000',
      bedrooms: 4,
      bathrooms: 3,
      area: 280,
      type: 'apartment',
      isFeatured: true,
      photo: 'https://images.unsplash.com/photo-1567496898669-ee935f5f647a?w=800&q=80',
      address: { id: 16, city: 'Skopje', street: 'Bul. Ilinden 88', country: 'Macedonia' }
    }
  ];

  constructor() {}

  async getAllProperties(): Promise<Property[]> {
  try {
    const result = await fetchWithErrorHandling<any[]>(this.url, this.router);
    return result.map((p: any) => this.mapToProperty(p));
  } catch (error) {
    console.warn('API not available, using mock data');
    return this.mockProperties;
  }
}

  async getPropertyById(id: number): Promise<Property | undefined> {
  try {
    const result = await fetchWithErrorHandling<any>(`${this.url}/${id}`, this.router);
    return this.mapToProperty(result);
  } catch (error: any) {
    const mock = this.mockProperties.find(p => p.id === id);
    if (mock) return mock;
    return undefined;
  }
}
private mapToProperty(p: any): Property {
  return {
    id: p.id,
    title: p.title,
    description: p.description,
    price: p.price,
    photo: p.photo || 'https://images.unsplash.com/photo-1600596542815-ffad4c1539a9?w=800&q=80',
    bedrooms: p.bedrooms,
    bathrooms: p.bathrooms,
    area: p.area,
    type: p.type || 'apartment',
    isFeatured: p.isFeatured,
    floor: p.floor,
    totalFloors: p.totalFloors,
    yearBuilt: p.yearBuilt,
    parkingSpots: p.parkingSpots,
    heatingType: p.heatingType,
    hasGarage: p.hasGarage,
    hasElevator: p.hasElevator,
    hasBalcony: p.hasBalcony,
    hasPool: p.hasPool,
    hasInternet: p.hasInternet,
    isFurnished: p.isFurnished,
    hasAirConditioning: p.hasAirConditioning,
    hasSecurity: p.hasSecurity,
    address: {
      id: p.addressId || 0,
      city: p.city || '',
      street: p.street || '',
      country: p.country || ''
    }
  };
}

  async searchProperties(query: string, filters: any): Promise<Property[]> {
  try {
    const params = new URLSearchParams();
    if (query) params.append('query', query);
    if (filters.type && filters.type !== 'all') params.append('type', filters.type);
    if (filters.minPrice) params.append('minPrice', filters.minPrice.toString());
    if (filters.maxPrice) params.append('maxPrice', filters.maxPrice.toString());
    if (filters.bedrooms && filters.bedrooms !== 'any') params.append('bedrooms', filters.bedrooms.toString());

    const result = await fetchWithErrorHandling<any[]>(
      `${this.url}/search?${params.toString()}`,
      this.router
    );
    return result.map((p: any) => this.mapToProperty(p));
  } catch (error) {
    console.warn('Search API not available, using local filter');
    let results = this.mockProperties;

    if (query) {
      const q = query.toLowerCase();
      results = results.filter(p =>
        p.title.toLowerCase().includes(q) ||
        p.address.city.toLowerCase().includes(q) ||
        p.type.toLowerCase().includes(q)
      );
    }

    if (filters.type && filters.type !== 'all') {
      results = results.filter(p => p.type === filters.type);
    }

    if (filters.minPrice) {
      results = results.filter(p => parseInt(p.price.replace(/[^0-9]/g, '')) >= filters.minPrice);
    }

    if (filters.maxPrice) {
      results = results.filter(p => parseInt(p.price.replace(/[^0-9]/g, '')) <= filters.maxPrice);
    }

    if (filters.bedrooms && filters.bedrooms !== 'any') {
      results = results.filter(p => p.bedrooms >= parseInt(filters.bedrooms));
    }

    return Promise.resolve(results);
  }
}

  async submitContactForm(name: string, email: string, phone: string, message: string, propertyId?: number) {
  try {
    await fetch('https://localhost:7112/api/inquiry', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({
        name: name,
        email: email,
        phone: phone,
        message: message,
        propertyId: propertyId
      })
    });
    console.log('Inquiry submitted successfully');
  } catch (error) {
    console.error('Error submitting inquiry:', error);
  }
}
}
