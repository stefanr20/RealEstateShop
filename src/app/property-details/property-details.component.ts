import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { PropertyService } from '../property.service';
import { FavoritesService } from '../favorites.service';
import { Property } from '../property';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-property-details',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './property-details.component.html',
  styleUrls: ['./property-details.component.css']
})
export class PropertyDetailsComponent {
  route = inject(ActivatedRoute);
  propertyService = inject(PropertyService);
  favoritesService = inject(FavoritesService);
  sanitizer = inject(DomSanitizer);
  authService = inject(AuthService);

  property: Property | undefined;
  submitted = false;
  copied = false;
  activePhoto = 0;
  lightboxOpen = false;
  lightboxIndex = 0;
  mapUrl: SafeResourceUrl = '';

  get galleryPhotos(): string[] {
    return this.property?.photos?.length ? this.property.photos : [this.property?.photo ?? ''];
  }

  setPhoto(index: number) {
    this.activePhoto = index;
  }

  openLightbox(index: number) {
    this.lightboxIndex = index;
    this.lightboxOpen = true;
  }

  closeLightbox() {
    this.lightboxOpen = false;
  }

  prevPhoto() {
    this.lightboxIndex = (this.lightboxIndex - 1 + this.galleryPhotos.length) % this.galleryPhotos.length;
  }

  nextPhoto() {
    this.lightboxIndex = (this.lightboxIndex + 1) % this.galleryPhotos.length;
  }

  contactForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    phone: new FormControl(''),
    message: new FormControl('', [Validators.required])
  });

  constructor() {
  const id = Number(this.route.snapshot.params['id']);
  this.propertyService.getPropertyById(id).then(property => {
    if (!property) {
      return;
    }
    this.property = property;

    const user = this.authService.currentUser();
    if (user) {
      this.contactForm.patchValue({
        name: user.username,
        email: user.email
      });
    }

    if (property?.address) {
      const address = `${property.address.street}, ${property.address.city}, Macedonia`;
      
      fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodeURIComponent(address)}`)
        .then(r => r.json())
        .then(results => {
          if (results.length > 0) {
            const lat = results[0].lat;
            const lon = results[0].lon;
            this.mapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
              `https://www.openstreetmap.org/export/embed.html?bbox=${parseFloat(lon)-0.01},${parseFloat(lat)-0.01},${parseFloat(lon)+0.01},${parseFloat(lat)+0.01}&layer=mapnik&marker=${lat},${lon}`
            );
          } else {
            this.mapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(
              `https://www.openstreetmap.org/export/embed.html?bbox=21.3,41.9,21.5,42.1&layer=mapnik`
            );
          }
        });
    }
  });
}

get hasAmenities(): boolean {
  return !!(this.property?.hasGarage || this.property?.hasElevator ||
    this.property?.hasBalcony || this.property?.hasPool ||
    this.property?.hasInternet || this.property?.isFurnished ||
    this.property?.hasAirConditioning || this.property?.hasSecurity);
}

  get isFav(): boolean {
    return this.property ? this.favoritesService.isFavorite(this.property.id) : false;
  }

  toggleFav() {
    if (this.property) this.favoritesService.toggleFavorite(this.property);
  }

  shareProperty() {
    navigator.clipboard.writeText(window.location.href);
    this.copied = true;
    setTimeout(() => this.copied = false, 2000);
  }

  async submitContact() {
    if (this.contactForm.valid && this.property) {
      await this.propertyService.submitContactForm(
        this.contactForm.value.name ?? '',
        this.contactForm.value.email ?? '',
        this.contactForm.value.phone ?? '',
        this.contactForm.value.message ?? '',
        this.property.id
      );
      this.submitted = true;
    }
  }
}