import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormGroup, FormControl, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';
import { FavoritesService } from '../favorites.service';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  authService = inject(AuthService);
  router = inject(Router);

  activeTab = signal<'profile' | 'inquiries' | 'saved' | 'properties'>('profile');
  loading = signal(true);
  saving = signal(false);
  successMessage = signal('');
  errorMessage = signal('');
  inquiries = signal<any[]>([]);
  savedSearches = signal<any[]>([]);
  favoritesService = inject(FavoritesService);

  profileForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl(''),
    confirmPassword: new FormControl('')
  });

  private apiUrl = 'https://localhost:7112/api';

  ngOnInit() {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/auth']);
      return;
    }
    this.loadProfile();
    this.loadSavedSearches();
  }

  setTab(tab: 'profile' | 'inquiries' | 'saved' | 'properties') {
  this.activeTab.set(tab);
  if (tab === 'inquiries') this.loadInquiries();
  if (tab === 'saved') this.loadSavedSearches();
}

  async loadProfile() {
    this.loading.set(true);
    try {
      const token = this.authService.getToken();
      const response = await fetch(`${this.apiUrl}/auth/profile`, {
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (response.ok) {
        const user = await response.json();
        this.profileForm.patchValue({
          username: user.username,
          email: user.email
        });
      }
    } catch (error) {
      console.error('Failed to load profile', error);
    } finally {
      this.loading.set(false);
    }
  }

  async loadInquiries() {
    try {
      const token = this.authService.getToken();
      const response = await fetch(`${this.apiUrl}/inquiry/my`, {
        headers: { 'Authorization': `Bearer ${token}` }
      });
      if (response.ok) {
        const data = await response.json();
        this.inquiries.set(data);
      }
    } catch (error) {
      console.error('Failed to load inquiries', error);
    }
  }

  loadSavedSearches() {
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const key = user?.id ? `savedSearches_${user.id}` : 'savedSearches_guest';
    const stored = localStorage.getItem(key);
    this.savedSearches.set(stored ? JSON.parse(stored) : []);
  }

  removeSavedSearch(index: number) {
  const user = JSON.parse(localStorage.getItem('user') || '{}');
  const key = user?.id ? `savedSearches_${user.id}` : 'savedSearches_guest';
  const current = this.savedSearches();
  current.splice(index, 1);
  localStorage.setItem(key, JSON.stringify(current));
  this.savedSearches.set([...current]);
}

  applySearch(search: any) {
    this.router.navigate(['/'], { queryParams: search });
  }

  async saveProfile() {
    if (this.profileForm.invalid) return;

    const { password, confirmPassword } = this.profileForm.value;
    if (password && password !== confirmPassword) {
      this.errorMessage.set('Passwords do not match.');
      return;
    }

    this.saving.set(true);
    this.successMessage.set('');
    this.errorMessage.set('');

    try {
      const token = this.authService.getToken();
      const body: any = {
        username: this.profileForm.value.username,
        email: this.profileForm.value.email
      };
      if (password) body.password = password;

      const response = await fetch(`${this.apiUrl}/auth/profile`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(body)
      });

      if (response.ok) {
        this.successMessage.set('Profile updated successfully.');
        this.profileForm.patchValue({ password: '', confirmPassword: '' });
        const stored = JSON.parse(localStorage.getItem('user') || '{}');
        stored.username = body.username;
        stored.email = body.email;
        localStorage.setItem('user', JSON.stringify(stored));
        this.authService.currentUser.set(stored);
      } else {
        this.errorMessage.set('Failed to update profile. Please try again.');
      }
    } catch (error) {
      this.errorMessage.set('Something went wrong. Please try again.');
    } finally {
      this.saving.set(false);
    }
  }

  formatDate(date: string): string {
    return new Date(date).toLocaleDateString('en-GB', {
      day: 'numeric', month: 'short', year: 'numeric'
    });
  }
}