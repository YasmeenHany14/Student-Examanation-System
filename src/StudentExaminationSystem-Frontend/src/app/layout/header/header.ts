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
  navLinks: { label: string, routerLink: any, icon?: string }[] = [];

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
        { label: 'Students', routerLink: '/students', icon: 'pi pi-users' },
        { label: 'Logout', routerLink: '', icon: 'pi pi-sign-out' }
      ];
    } else if (this.user.role === UserRole.STUDENT) {
      this.navLinks = [
        { label: 'Home', routerLink: '/home', icon: 'pi pi-home' },
        { label: 'Profile', routerLink: '/profile', icon: 'pi pi-user' },
        { label: 'Take Exam', routerLink: '/exam', icon: 'pi pi-pencil' },
        { label: 'Logout', routerLink: '', icon: 'pi pi-sign-out' }
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
