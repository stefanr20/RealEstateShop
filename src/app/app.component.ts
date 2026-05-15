import { Component, inject, effect } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FavoritesService } from './favorites.service';
import { AuthService } from './auth.service';
import { Router } from '@angular/router';



@Component({
  standalone: true,
  selector: 'app-root',
  imports: [RouterModule, CommonModule],
  template: `
    <div class="app-wrapper">
      <nav class="navbar">
        <a routerLink="/" class="nav-brand">
          <img src="assets/logo.png" alt="VeloraEstate" class="brand-logo">
          <span class="brand-name">VeloraEstate</span>
        </a>
        <div class="nav-links">
  <a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{exact: true}" class="nav-link">Properties</a>
  <a routerLink="/admin" class="nav-link" *ngIf="isAdmin">Admin</a>
  <a routerLink="/login" class="nav-link login-link" *ngIf="!isLoggedIn">Sign In</a>
  <div class="user-dropdown" *ngIf="isLoggedIn">
    <button class="user-trigger" (click)="toggleDropdown()">
      <div class="user-avatar">
        {{ authService.currentUser()?.username?.charAt(0)?.toUpperCase() }}
      </div>
      <span class="user-name">{{ authService.currentUser()?.username }}</span>
      <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="12" height="12">
        <polyline points="6 9 12 15 18 9"/>
      </svg>
    </button>
    <div class="dropdown-menu" *ngIf="dropdownOpen">
      <a routerLink="/profile" class="dropdown-item" (click)="closeDropdown()">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="14" height="14">
          <path d="M20 21v-2a4 4 0 0 0-4-4H8a4 4 0 0 0-4 4v2"/>
          <circle cx="12" cy="7" r="4"/>
        </svg>
        My Profile
      </a>
      <div class="dropdown-divider"></div>
      <button class="dropdown-item logout" (click)="logout()">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="14" height="14">
          <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/>
          <polyline points="16 17 21 12 16 7"/>
          <line x1="21" y1="12" x2="9" y2="12"/>
        </svg>
        Sign Out
      </button>
    </div>
  </div>
  <a (click)="goToFavs()" class="nav-link fav-link" style="cursor:pointer">
    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="16" height="16">
      <path d="M20.84 4.61a5.5 5.5 0 0 0-7.78 0L12 5.67l-1.06-1.06a5.5 5.5 0 0 0-7.78 7.78l1.06 1.06L12 21.23l7.78-7.78 1.06-1.06a5.5 5.5 0 0 0 0-7.78z"/>
    </svg>
    Saved
    <span class="fav-count" *ngIf="favCount > 0">{{ favCount }}</span>
  </a>
</div>
        <button class="burger-btn" (click)="toggleMenu()">
          <svg *ngIf="!menuOpen" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="24" height="24">
            <line x1="3" y1="6" x2="21" y2="6"/>
            <line x1="3" y1="12" x2="21" y2="12"/>
            <line x1="3" y1="18" x2="21" y2="18"/>
          </svg>
          <svg *ngIf="menuOpen" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="24" height="24">
            <line x1="18" y1="6" x2="6" y2="18"/>
            <line x1="6" y1="6" x2="18" y2="18"/>
          </svg>
        </button>
      </nav>

      <!-- MOBILE MENU OVERLAY -->
      <div class="menu-overlay" [class.open]="menuOpen" (click)="closeMenu()"></div>

      <!-- MOBILE MENU PANEL -->
      <div class="menu-panel" [class.open]="menuOpen">
        <div class="menu-header">
          <a routerLink="/" class="nav-brand" (click)="closeMenu()">
            <img src="assets/logo.png" alt="VeloraEstate" class="brand-logo">
            <span class="brand-name">VeloraEstate</span>
          </a>
          <button class="menu-close" (click)="closeMenu()">✕</button>
        </div>

        <nav class="menu-nav">
  <a routerLink="/" class="menu-link" (click)="closeMenu()">Properties</a>
  <a routerLink="/about" class="menu-link" (click)="closeMenu()">About Us</a>
  <a routerLink="/contact" class="menu-link" (click)="closeMenu()">Contact</a>
  <a routerLink="/admin" class="menu-link" *ngIf="isAdmin" (click)="closeMenu()">Admin</a>
  <a routerLink="/login" class="menu-link" *ngIf="!isLoggedIn" (click)="closeMenu()">Sign In</a>
  <a routerLink="/profile" class="menu-link" *ngIf="isLoggedIn" (click)="closeMenu()">My Profile</a>
  <a class="menu-link" *ngIf="isLoggedIn" (click)="logout()" style="cursor:pointer">Sign Out</a>
</nav>

<div class="menu-lang">
  <button class="lang-btn" [class.active]="currentLang === 'EN'" (click)="setLang('EN')">EN</button>
  <span class="lang-divider">|</span>
  <button class="lang-btn" [class.active]="currentLang === 'MK'" (click)="setLang('MK')">MK</button>
</div>

        <div class="menu-social">
          <a href="#" class="social-link">
            <svg viewBox="0 0 24 24" fill="currentColor" width="22" height="22">
              <path d="M18 2h-3a5 5 0 0 0-5 5v3H7v4h3v8h4v-8h3l1-4h-4V7a1 1 0 0 1 1-1h3z"/>
            </svg>
          </a>
          <a href="#" class="social-link">
            <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" width="22" height="22">
              <rect x="2" y="2" width="20" height="20" rx="5" ry="5"/>
              <circle cx="12" cy="12" r="4"/>
              <circle cx="17.5" cy="6.5" r="1" fill="currentColor"/>
            </svg>
          </a>
          <a href="#" class="social-link">
            <svg viewBox="0 0 24 24" fill="currentColor" width="22" height="22">
              <path d="M16 8a6 6 0 0 1 6 6v7h-4v-7a2 2 0 0 0-2-2 2 2 0 0 0-2 2v7h-4v-7a6 6 0 0 1 6-6z"/>
              <rect x="2" y="9" width="4" height="12"/><circle cx="4" cy="4" r="2"/>
            </svg>
          </a>
        </div>
      </div>

      <main>
        <router-outlet></router-outlet>
      </main>

      <footer class="footer">
        <div class="footer-content">
          <div class="footer-brand">
            <img src="assets/logo.png" alt="VeloraEstate" class="brand-logo">
            <span class="brand-name">VeloraEstate</span>
            <p class="footer-tagline">Premium Properties in Macedonia</p>
          </div>
          <div class="footer-links">
            <h4>Navigation</h4>
            <a routerLink="/">Properties</a>
            <a routerLink="/about">About Us</a>
            <a routerLink="/contact">Contact</a>
            <a routerLink="/login" *ngIf="!isLoggedIn">Sign In</a>
            <a routerLink="/profile" *ngIf="isLoggedIn">My Profile</a>
          </div>
          <div class="footer-legal">
            <h4>Legal</h4>
            <a routerLink="/terms">Terms & Conditions</a>
            <a routerLink="/privacy">Privacy Policy</a>
          </div>
          <div class="footer-contact">
            <h4>Contact</h4>
            <p>info@veloraestate.com</p>
            <p>+389 70 000 000</p>
            <p>Skopje, Macedonia</p>
          </div>
        </div>
        <div class="footer-bottom">
          <p>© 2026 VeloraEstate · All rights reserved</p>
        </div>
      </footer>

      <button class="back-to-top" (click)="scrollToTop()" [class.visible]="showBackToTop">
        <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2.5" width="20" height="20">
          <polyline points="18 15 12 9 6 15"/>
        </svg>
      </button>
    </div>
  `,
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  favoritesService = inject(FavoritesService);
  authService = inject(AuthService);
  router = inject(Router);
  showBackToTop = false;
  menuOpen = false;
  dropdownOpen = false;
  

  constructor() {
  window.addEventListener('scroll', () => {
    this.showBackToTop = window.scrollY > 400;
  });

  // Reload favorites when user changes
  effect(() => {
  const user = this.authService.currentUser();
  this.dropdownOpen = false;
  if (user) {
    this.favoritesService.loadForUser();
  }
}, { allowSignalWrites: true });
}

  get favCount(): number {
    return this.favoritesService.getFavoritesCount();
  }

  get isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  get isAdmin(): boolean {
    const user = this.authService.currentUser();
    return user && user.role === 'admin';
  }

  toggleMenu() {
    this.menuOpen = !this.menuOpen;
  }

  toggleDropdown() {
  this.dropdownOpen = !this.dropdownOpen;
  if (this.dropdownOpen) {
    setTimeout(() => {
      document.addEventListener('click', this.onClickOutside);
    }, 0);
  }
}

onClickOutside = (event: MouseEvent) => {
  const dropdown = document.querySelector('.user-dropdown');
  if (dropdown && !dropdown.contains(event.target as Node)) {
    this.dropdownOpen = false;
    document.removeEventListener('click', this.onClickOutside);
  }
}

closeDropdown() {
  this.dropdownOpen = false;
  document.removeEventListener('click', this.onClickOutside);
}

  closeMenu() {
    this.menuOpen = false;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(['/login']);
    this.closeMenu();
  }

  scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  goToFavs() {
  this.router.navigateByUrl('/', { skipLocationChange: true }).then(() => {
    this.router.navigate(['/'], { fragment: 'saved' });
  });
}
  currentLang = 'EN';

setLang(lang: string) {
  this.currentLang = lang;
}

}

