import { Injectable, signal, inject } from '@angular/core';
import { Property } from './property';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class FavoritesService {
  private authService = inject(AuthService);
  private favorites = signal<Property[]>([]);

  private getKey(): string {
    const user = this.authService.currentUser();
    return user ? `favorites_${user.id}` : 'favorites_guest';
  }

  constructor() {
    const stored = localStorage.getItem(this.getKey());
    if (stored) {
      this.favorites.set(JSON.parse(stored));
    }
  }

  loadForUser() {
    const stored = localStorage.getItem(this.getKey());
    this.favorites.set(stored ? JSON.parse(stored) : []);
  }

  getFavorites() {
    return this.favorites;
  }

  isFavorite(id: number): boolean {
    return this.favorites().some(p => p.id === id);
  }

  toggleFavorite(property: Property) {
    if (this.isFavorite(property.id)) {
      this.favorites.update(favs => favs.filter(p => p.id !== property.id));
    } else {
      this.favorites.update(favs => [...favs, property]);
    }
    localStorage.setItem(this.getKey(), JSON.stringify(this.favorites()));
  }

  getFavoritesCount(): number {
    return this.favorites().length;
  }
}