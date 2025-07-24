import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../core/services/auth.service';
import {User} from '../../core/models/user.model';
import {UserRole} from '../../core/enums/user-role';
import {Menubar} from 'primeng/menubar';

@Component({
  selector: 'app-header',
  imports: [
    Menubar,
    RouterLink
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header {
  user: User | null = null;
  navLinks: { label: string, route: string, icon?: string }[] = [];

  constructor(private authService: AuthService, private router: Router) {
    this.authService.currentUser$.subscribe(user => {
      this.user = user;
      this.setNavLinks();
    });
  }

  setNavLinks() {
    if (!this.user) {
      this.navLinks = [];
      return;
    }
    if (this.user.role === UserRole.ADMIN) {
      this.navLinks = [
        { label: 'Home', route: '/home/dashboard', icon: 'pi pi-home' },
        { label: 'Subjects', route: '/subjects', icon: 'pi pi-book' },
        { label: 'Questions', route: '/questions', icon: 'pi pi-question' },
        { label: 'Students', route: '/students', icon: 'pi pi-users' },
        { label: 'Logout', route: '', icon: 'pi pi-sign-out' }
      ];
    } else if (this.user.role === UserRole.STUDENT) {
      this.navLinks = [
        { label: 'Home', route: '/home', icon: 'pi pi-home' },
        { label: 'Profile', route: '/profile', icon: 'pi pi-user' },
        { label: 'Take Exam', route: '/exam', icon: 'pi pi-pencil' },
        { label: 'Logout', route: '', icon: 'pi pi-sign-out' }
      ];
    }
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }

  showNavBar(): boolean {
    const route = this.router.url;
    return !(route.startsWith('/login') || route.startsWith('/register'));
  }
}
