import { Component, Input, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Property } from '../property';
import { FavoritesService } from '../favorites.service';
import * as L from 'leaflet';

@Component({
  selector: 'app-property-card',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <article class="card" [class.featured]="property.isFeatured">
      <div class="card-image-wrap">
        <img [src]="property.photo" [alt]="property.title" class="card-image">
        <span class="property-type">{{ property.type }}</span>
        <button class="fav-btn" (click)="toggleFav($event)" [class.active]="isFav">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="16" height="16">
            <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/>
          </svg>
        </button>
        <div class="price-tag">{{ property.price }}</div>
      </div>
      <div class="card-body">
        <h3 class="card-title">{{ property.title }}</h3>
        <p class="card-location">
          <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="14" height="14">
            <path d="M21 10c0 7-9 13-9 13s-9-6-9-13a9 9 0 0 1 18 0z"/><circle cx="12" cy="10" r="3"/>
          </svg>
          {{ property.address.city }}, {{ property.address.country }}
        </p>
        <div class="card-stats">
          <span><strong>{{ property.bedrooms }}</strong> bed</span>
          <span class="divider">·</span>
          <span><strong>{{ property.bathrooms }}</strong> bath</span>
          <span class="divider">·</span>
          <span><strong>{{ property.area }}</strong> m²</span>
        </div>
        <a [routerLink]="['/property', property.id]" class="view-btn">View Details</a>
      </div>
    </article>
  `,
  styleUrls: ['./property-card.component.css']
})
export class PropertyCardComponent {
  @Input() property!: Property;
  favoritesService = inject(FavoritesService);

  get isFav(): boolean {
    return this.favoritesService.isFavorite(this.property.id);
  }

  toggleFav(event: Event) {
    event.preventDefault();
    event.stopPropagation();
    this.favoritesService.toggleFavorite(this.property);
  }
}
