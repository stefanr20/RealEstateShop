import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { PropertyCardComponent } from '../property-card/property-card.component';
import { SkeletonCardComponent } from '../skeleton-card/skeleton-card.component';
import { Property } from '../property';
import { PropertyService } from '../property.service';
import { FavoritesService } from '../favorites.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule, PropertyCardComponent, SkeletonCardComponent],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  propertyService = inject(PropertyService);
  favoritesService = inject(FavoritesService);
  route = inject(ActivatedRoute); 

  allProperties: Property[] = [];
  filteredProperties: Property[] = [];
  showFavoritesOnly = false;
  isLoading = true;
  searchApplied = false;

  searchQuery = '';
  sortOption = 'default';
  filters = {
    type: 'all',
    minPrice: null as number | null,
    maxPrice: null as number | null,
    bedrooms: 'any'
  };

  propertyTypes = ['all', 'apartment', 'house', 'villa', 'studio'];
  bedroomOptions = ['any', '1', '2', '3', '4', '5'];
  sortOptions = [
    { value: 'default', label: 'Default' },
    { value: 'price-asc', label: 'Price: Low to High' },
    { value: 'price-desc', label: 'Price: High to Low' },
    { value: 'area-desc', label: 'Largest First' },
    { value: 'bedrooms-desc', label: 'Most Bedrooms' },
  ];

  skeletons = Array(8).fill(0);
  currentPage = 1;
  pageSize = 9;
  totalPages = 1;
  totalProperties = 0;

  searchSaved = false;
  newsletterEmail = '';
newsletterSubmitted = false;

async subscribe() {
  if (!this.newsletterEmail) return;
  try {
    const response = await fetch('https://localhost:7112/api/subscriber', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email: this.newsletterEmail })
    });
    if (response.ok) {
      this.newsletterSubmitted = true;
    }
  } catch (error) {
    console.error('Subscribe error:', error);
  }
}

  constructor() {
  this.loadPage(1);
  this.route.fragment.subscribe(fragment => {
    if (fragment === 'saved') {
      this.showFavoritesOnly = true;
      setTimeout(() => {
        document.getElementById('results')?.scrollIntoView({ behavior: 'smooth' });
      }, 300);
    }
  });
}

  loadPage(page: number) {
    this.isLoading = true;
    fetch(`https://localhost:7112/api/property/paged?page=${page}&pageSize=${this.pageSize}`)
      .then(r => r.json())
      .then(result => {
        this.allProperties = result.items.map((p: any) => ({
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
          address: {
            id: 0,
            city: p.city || '',
            street: p.street || '',
            country: p.country || ''
          }
        }));
        this.filteredProperties = this.allProperties;
        this.currentPage = result.page;
        this.totalPages = result.totalPages;
        this.totalProperties = result.total;
        this.isLoading = false;
      })
      .catch(() => {
        this.propertyService.getAllProperties().then(properties => {
          this.allProperties = properties;
          this.filteredProperties = properties;
          this.isLoading = false;
        });
      });
  }

  nextPage() {
    if (this.currentPage < this.totalPages) {
      this.loadPage(this.currentPage + 1);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  prevPage() {
    if (this.currentPage > 1) {
      this.loadPage(this.currentPage - 1);
      window.scrollTo({ top: 0, behavior: 'smooth' });
    }
  }

  get featuredProperties(): Property[] {
  const list = this.searchApplied ? this.filteredProperties : this.allProperties;
  return list.filter(p => p.isFeatured);
}

  get displayedProperties(): Property[] {
    let list = this.showFavoritesOnly
      ? this.favoritesService.getFavorites()()
      : this.filteredProperties;
    return this.sortProperties(list);
  }

  sortProperties(list: Property[]): Property[] {
    switch (this.sortOption) {
      case 'price-asc':
        return [...list].sort((a, b) => this.parsePrice(a.price) - this.parsePrice(b.price));
      case 'price-desc':
        return [...list].sort((a, b) => this.parsePrice(b.price) - this.parsePrice(a.price));
      case 'area-desc':
        return [...list].sort((a, b) => b.area - a.area);
      case 'bedrooms-desc':
        return [...list].sort((a, b) => b.bedrooms - a.bedrooms);
      default:
        return list;
    }
  }

  parsePrice(price: string): number {
    return parseInt(price.replace(/[^0-9]/g, ''));
  }

  get favCount(): number {
    return this.favoritesService.getFavoritesCount();
  }

  async applyFilters() {
  this.isLoading = true;
  this.showFavoritesOnly = false;
  this.searchApplied = true;
  

  try {
    const params = new URLSearchParams();
    if (this.searchQuery) params.append('query', this.searchQuery);
    if (this.filters.type && this.filters.type !== 'all') params.append('type', this.filters.type);
    if (this.filters.minPrice) params.append('minPrice', this.filters.minPrice.toString());
    if (this.filters.maxPrice) params.append('maxPrice', this.filters.maxPrice.toString());
    if (this.filters.bedrooms && this.filters.bedrooms !== 'any') params.append('bedrooms', this.filters.bedrooms.toString());

    const data = await fetch(`https://localhost:7112/api/property/search?${params.toString()}`);
    const result = await data.json();
    this.allProperties = result.map((p: any) => ({
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
      address: {
        id: 0,
        city: p.city || p.City || '',
        street: p.street || p.Street || '',
        country: p.country || p.Country || ''
      }
    }));
    this.filteredProperties = this.allProperties;
    this.totalPages = 1;
    this.currentPage = 1;
  } catch (error) {
    console.warn('Search API not available');
  }

  this.isLoading = false;
}

  clearFilters() {
  this.searchQuery = '';
  this.sortOption = 'default';
  this.filters = { type: 'all', minPrice: null, maxPrice: null, bedrooms: 'any' };
  this.showFavoritesOnly = false;
  this.loadPage(1);
  this.searchApplied = false;
}

saveSearch() {
  const search = {
    name: this.searchQuery || 'Search ' + new Date().toLocaleDateString(),
    query: this.searchQuery,
    type: this.filters.type,
    minPrice: this.filters.minPrice,
    maxPrice: this.filters.maxPrice,
    bedrooms: this.filters.bedrooms
  };

  const user = JSON.parse(localStorage.getItem('user') || '{}');
  const key = user?.id ? `savedSearches_${user.id}` : 'savedSearches_guest';
  const existing = JSON.parse(localStorage.getItem(key) || '[]');
  existing.push(search);
  localStorage.setItem(key, JSON.stringify(existing));
  this.searchSaved = true;
  setTimeout(() => this.searchSaved = false, 2000);
}

  toggleFavoritesView() {
    this.showFavoritesOnly = !this.showFavoritesOnly;
  }
}