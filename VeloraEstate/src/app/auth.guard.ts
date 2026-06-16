import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const user = authService.currentUser();

   if (user && user.role === 'admin') {
    return true;
  }

  if (user && user.role !== 'admin') {
    router.navigate(['/']);
    return false;
  }

  router.navigate(['/login']);
  return false;
};