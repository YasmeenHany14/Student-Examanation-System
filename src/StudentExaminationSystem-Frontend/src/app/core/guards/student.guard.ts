import {inject, Injectable} from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class StudentGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.authService.isAuthenticated() && this.authService.isStudent()) {
      return true;
    }

    // Redirect to appropriate dashboard based on role or login
    if (this.authService.isAuthenticated()) {
      this.router.navigate(['/admin/dashboard']);
    } else {
      this.router.navigate(['/auth/login']);
    }
    return false;
  }
}

// const studentGuard = () => {
//   const authService = inject(AuthService);
//   const router = inject(Router);
//
//   if (authService.isAuthenticated() && authService.isStudent()) {
//     return true;
//   }
//
//   if (authService.isAuthenticated()) {
//     router.navigate(['/admin/dashboard']);
//   } else {
//     router.navigate(['/auth/login']);
//   }
//   return false;
// };
