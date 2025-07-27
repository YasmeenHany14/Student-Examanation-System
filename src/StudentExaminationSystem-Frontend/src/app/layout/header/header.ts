import { Component } from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../core/services/auth.service';
import {User} from '../../core/models/user.model';
import {UserRole} from '../../core/enums/user-role';
import {Menubar} from 'primeng/menubar';
import {routes} from '../../core/constants/routs';

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
  navLinks: { label: string, routerLink?: any, icon?: string, command?: () => void }[] = [];

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
        { label: 'Home', routerLink: '/home/dashboard', icon: 'pi pi-home' },
        { label: 'Subjects', routerLink: '/home/subjects', icon: 'pi pi-book' },
        { label: 'Questions', routerLink: '/home/questions', icon: 'pi pi-question' },
        { label: 'Difficulty Profiles', routerLink: '/home/difficulty-profiles', icon: 'pi pi-chart-line' },
        { label: 'Students', routerLink: '/home/students', icon: 'pi pi-users' },
        { label: 'Logout', icon: 'pi pi-sign-out', command: () => this.onLogout() }
      ];
    } else if (this.user.role === UserRole.STUDENT) {
      this.navLinks = [
        { label: 'Home', routerLink: '/home/exam', icon: 'pi pi-home' },
        { label: 'Profile', routerLink: '/profile', icon: 'pi pi-user' },
        { label: 'Take Exam', routerLink: '/home/take-exam', icon: 'pi pi-pencil' },
        { label: 'Logout', icon: 'pi pi-sign-out', command: () => this.onLogout() }
      ];
    }
  }

  onLogout() {
    this.authService.logout();
    this.router.navigate([routes.authLogin]);
  }

  goToProfile() {
    this.router.navigate(['/profile']);
  }

  showNavBar(): boolean {
    const route = this.router.url;
    return !(route.startsWith('/login') || route.startsWith('/register'));
  }
}
