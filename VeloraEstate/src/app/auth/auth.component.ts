import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-auth',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  authService = inject(AuthService);
  router = inject(Router);

  isLogin = true;
  submitted = false;
  errorMessage = '';
  loading = false;

  loginForm = new FormGroup({
    username: new FormControl('', [Validators.required]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)])
  });

  registerForm = new FormGroup({
    name: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required, Validators.minLength(6)]),
    confirm: new FormControl('', [Validators.required])
  });

  toggleMode() {
    this.isLogin = !this.isLogin;
    this.submitted = false;
    this.errorMessage = '';
  }

  async submitLogin() {
    if (this.loginForm.invalid) return;
    this.loading = true;
    this.errorMessage = '';

    const success = await this.authService.login(
      this.loginForm.value.username ?? '',
      this.loginForm.value.password ?? ''
    );

    this.loading = false;

    if (success) {
      this.router.navigate(['/']);
    } else {
      this.errorMessage = 'Invalid username or password';
    }
  }

  async submitRegister() {
    if (this.registerForm.invalid) return;
    this.loading = true;
    this.errorMessage = '';

    const success = await this.authService.register(
      this.registerForm.value.name ?? '',
      this.registerForm.value.email ?? '',
      this.registerForm.value.password ?? ''
    );

    this.loading = false;

    if (success) {
      this.submitted = true;
    } else {
      this.errorMessage = 'Username already exists or registration failed';
    }
  }
}