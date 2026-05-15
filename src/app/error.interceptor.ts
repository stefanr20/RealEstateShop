import { Router } from '@angular/router';

export async function fetchWithErrorHandling<T>(
  url: string,
  router: Router,
  options?: RequestInit
): Promise<T> {
  const response = await fetch(url, options);

  if (!response.ok) {
    let message = 'An unexpected error occurred.';
    try {
      const body = await response.json();
      message = body.message ?? message;
    } catch { }

    switch (response.status) {
      case 401: router.navigate(['/401']); break;
      case 403: router.navigate(['/403']); break;
      case 404: router.navigate(['/404']); break;
      default:  router.navigate(['/500']); break;
    }

    throw new Error(message);
  }

  return response.json() as Promise<T>;
}