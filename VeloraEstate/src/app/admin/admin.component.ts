import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { PropertyService } from '../property.service';
import { Property } from '../property';
import { AdminStatsCardComponent } from './admin-stats-card/admin-stats-card.component';
import { AdminPropertyRowComponent } from './admin-property-row/admin-property-row.component';

@Component({
  selector: 'app-admin',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule, ReactiveFormsModule, AdminStatsCardComponent, AdminPropertyRowComponent],
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent {
  propertyService = inject(PropertyService);
  properties: Property[] = [];
  showForm = false;
  editingId: number | null = null;
  deleteConfirmId: number | null = null;
  searchQuery = '';

  stats = [
    { value: 0, label: 'Total Properties' },
    { value: 0, label: 'Featured' },
    { value: 0, label: 'Villas' },
    { value: 0, label: 'Apartments' }
  ];

  propertyForm = new FormGroup({
  title: new FormControl('', [Validators.required]),
  description: new FormControl('', [Validators.required]),
  price: new FormControl('', [Validators.required]),
  type: new FormControl('apartment', [Validators.required]),
  bedrooms: new FormControl(1, [Validators.required]),
  bathrooms: new FormControl(1, [Validators.required]),
  area: new FormControl(50, [Validators.required]),
  photo: new FormControl('', [Validators.required]),
  city: new FormControl('', [Validators.required]),
  street: new FormControl('', [Validators.required]),
  country: new FormControl('Macedonia', [Validators.required]),
  isFeatured: new FormControl(false),
  floor: new FormControl<number | null>(null),
  totalFloors: new FormControl<number | null>(null),
  yearBuilt: new FormControl<number | null>(null),
  parkingSpots: new FormControl<number | null>(null),
  heatingType: new FormControl(''),
  hasGarage: new FormControl(false),
  hasElevator: new FormControl(false),
  hasBalcony: new FormControl(false),
  hasPool: new FormControl(false),
  hasInternet: new FormControl(false),
  isFurnished: new FormControl(false),
  hasAirConditioning: new FormControl(false),
  hasSecurity: new FormControl(false)
});

  users: any[] = [];

loadUsers() {
  fetch('https://localhost:7112/api/auth/users')
    .then(r => r.json())
    .then(users => this.users = users)
    .catch(err => console.error('Error loading users:', err));
}

async promoteUser(id: number) {
  await fetch(`https://localhost:7112/api/auth/users/${id}/role`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ role: 'admin' })
  });
  this.loadUsers();
}

async demoteUser(id: number) {
  await fetch(`https://localhost:7112/api/auth/users/${id}/role`, {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ role: 'user' })
  });
  this.loadUsers();
}

inquiries: any[] = [];
replyTexts: { [id: number]: string } = {};
replySuccess: { [id: number]: boolean } = {};

loadInquiries() {
  const token = localStorage.getItem('token');
  fetch('https://localhost:7112/api/inquiry', {
    headers: { 'Authorization': `Bearer ${token}` }
  })
    .then(r => r.json())
    .then(data => this.inquiries = data)
    .catch(err => console.error('Error loading inquiries:', err));
}

async sendReply(inquiryId: number) {
  const reply = this.replyTexts[inquiryId];
  if (!reply?.trim()) return;
  const token = localStorage.getItem('token');
  await fetch(`https://localhost:7112/api/inquiry/${inquiryId}/reply`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    },
    body: JSON.stringify({ reply })
  });
  this.replySuccess[inquiryId] = true;
  this.replyTexts[inquiryId] = '';
  setTimeout(() => this.replySuccess[inquiryId] = false, 3000);
  this.loadInquiries();
}

formatDate(date: string): string {
  return new Date(date).toLocaleDateString('en-GB', {
    day: 'numeric', month: 'short', year: 'numeric'
  });
}

  constructor() {
    this.loadProperties();
    this.loadUsers();
    this.loadInquiries();
  }

  loadProperties() {
  this.propertyService.getAllProperties().then(properties => {
    this.properties = properties;
    this.updateStats();
  });
}

  updateStats() {
    this.stats[0].value = this.properties.length;
    this.stats[1].value = this.properties.filter(p => p.isFeatured).length;
    this.stats[2].value = this.properties.filter(p => p.type === 'villa').length;
    this.stats[3].value = this.properties.filter(p => p.type === 'apartment').length;
  }

  get filteredProperties(): Property[] {
    if (!this.searchQuery) return this.properties;
    const q = this.searchQuery.toLowerCase();
    return this.properties.filter(p =>
      p.title.toLowerCase().includes(q) ||
      p.address.city.toLowerCase().includes(q) ||
      p.type.toLowerCase().includes(q)
    );
  }

  openAddForm() {
    this.editingId = null;
    this.propertyForm.reset({
      type: 'apartment',
      bedrooms: 1,
      bathrooms: 1,
      area: 50,
      country: 'Macedonia',
      isFeatured: false
    });
    this.showForm = true;
  }

  openEditForm(property: Property) {
  this.editingId = property.id;
  this.propertyForm.setValue({
    title: property.title,
    description: property.description,
    price: property.price,
    type: property.type,
    bedrooms: property.bedrooms,
    bathrooms: property.bathrooms,
    area: property.area,
    photo: property.photo,
    city: property.address.city,
    street: property.address.street,
    country: property.address.country,
    isFeatured: property.isFeatured ?? false,
    floor: property.floor ?? null,
    totalFloors: property.totalFloors ?? null,
    yearBuilt: property.yearBuilt ?? null,
    parkingSpots: property.parkingSpots ?? null,
    heatingType: property.heatingType ?? '',
    hasGarage: property.hasGarage ?? false,
    hasElevator: property.hasElevator ?? false,
    hasBalcony: property.hasBalcony ?? false,
    hasPool: property.hasPool ?? false,
    hasInternet: property.hasInternet ?? false,
    isFurnished: property.isFurnished ?? false,
    hasAirConditioning: property.hasAirConditioning ?? false,
    hasSecurity: property.hasSecurity ?? false
  });
  this.showForm = true;
}

  async submitForm() {
  if (this.propertyForm.invalid) return;
  const v = this.propertyForm.value;

  const payload = {
  title: v.title,
  description: v.description,
  price: v.price,
  photo: v.photo,
  bedrooms: v.bedrooms,
  bathrooms: v.bathrooms,
  area: v.area,
  type: v.type,
  isFeatured: v.isFeatured ?? false,
  floor: v.floor,
  totalFloors: v.totalFloors,
  yearBuilt: v.yearBuilt,
  parkingSpots: v.parkingSpots,
  heatingType: v.heatingType,
  hasGarage: v.hasGarage ?? false,
  hasElevator: v.hasElevator ?? false,
  hasBalcony: v.hasBalcony ?? false,
  hasPool: v.hasPool ?? false,
  hasInternet: v.hasInternet ?? false,
  isFurnished: v.isFurnished ?? false,
  hasAirConditioning: v.hasAirConditioning ?? false,
  hasSecurity: v.hasSecurity ?? false,
  address: {
    city: v.city,
    street: v.street,
    country: v.country
  }
};

  try {
    if (this.editingId !== null) {
      await fetch(`https://localhost:7112/api/property`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: this.editingId, ...payload })
      });
    } else {
      await fetch('https://localhost:7112/api/property', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payload)
      });
    }
    this.loadProperties();
  } catch (error) {
    console.error('Error saving property:', error);
  }

  this.showForm = false;
  this.editingId = null;
}

  confirmDelete(id: number) {
    this.deleteConfirmId = id;
  }

  async deleteProperty() {
  if (this.deleteConfirmId !== null) {
    try {
      await fetch(`https://localhost:7112/api/property/${this.deleteConfirmId}`, {
        method: 'DELETE'
      });
      this.loadProperties();
    } catch (error) {
      console.error('Error deleting property:', error);
    }
    this.deleteConfirmId = null;
  }
}

  cancelDelete() {
    this.deleteConfirmId = null;
  }

  closeForm() {
    this.showForm = false;
    this.editingId = null;
  }
}