import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.authService.isAuthenticated() && this.authService.isAdmin()) {
      return true;
    }

    // Redirect to appropriate dashboard based on role or login
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/student/dashboard']);
    } else {
      this.router.navigate(['/auth/login']);
    }
    return false;
  }
}
