import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7112/api/auth';
  
  currentUser = signal<any>(null);

  constructor() {
    const stored = localStorage.getItem('user');
    if (stored) {
      this.currentUser.set(JSON.parse(stored));
    }
  }

  async login(username: string, password: string): Promise<boolean> {
    try {
      const response = await fetch(`${this.apiUrl}/login`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
      });

      if (response.ok) {
        const user = await response.json();
        this.currentUser.set(user);
        localStorage.setItem('user', JSON.stringify(user));
        localStorage.setItem('token', user.token);
        return true;
      }
      return false;
    } catch (error) {
      console.error('Login error:', error);
      return false;
    }
  }

  async register(username: string, email: string, password: string): Promise<boolean> {
    try {
      const response = await fetch(`${this.apiUrl}/register`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, email, password })
      });
      return response.ok;
    } catch (error) {
      console.error('Register error:', error);
      return false;
    }
  }

  logout() {
    this.currentUser.set(null);
    localStorage.removeItem('user');
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return this.currentUser() !== null;
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }
}