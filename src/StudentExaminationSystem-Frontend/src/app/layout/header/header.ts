import {Component, DestroyRef, inject, OnInit, signal} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {AuthService} from '../../core/services/auth.service';
import {User} from '../../core/models/user.model';
import {UserRole} from '../../core/enums/user-role';
import {Menubar} from 'primeng/menubar';
import {routes} from '../../core/constants/routs';
import { BadgeModule } from 'primeng/badge';
import { OverlayBadgeModule } from 'primeng/overlaybadge';
import {NotificationService} from '../../core/services/notification.service';

@Component({
  selector: 'app-header',
  imports: [
    Menubar,
    RouterLink,
    BadgeModule,
    OverlayBadgeModule
  ],
  templateUrl: './header.html',
  styleUrl: './header.scss'
})
export class Header implements OnInit {
  user: User | null = null;
  navLinks: { label: string, routerLink?: any, icon?: string, command?: () => void }[] = [];
  notificationCount = signal<number>(0);
  private notificationService = inject(NotificationService);
  private destoryRef = inject(DestroyRef);

  constructor(private authService: AuthService, private router: Router) {
    this.authService.currentUser$.subscribe(user => {
      this.user = user;
      this.setNavLinks();
    });
  }

  async ngOnInit(): Promise<void> {
    await this.connectToNotificationService();
  }

  setNavLinks() {
    if (!this.user) {
      this.navLinks = [];
      return;
    }
    if (this.user.role === UserRole.ADMIN) {
      this.navLinks = [
        { label: 'Home', routerLink: '/home', icon: 'pi pi-home' },
        { label: 'Subjects', routerLink: '/home/subjects', icon: 'pi pi-book' },
        { label: 'Questions', routerLink: '/home/questions', icon: 'pi pi-question' },
        { label: 'Difficulty Profiles', routerLink: '/home/difficulty-profiles', icon: 'pi pi-chart-line' },
        { label: 'Students', routerLink: '/home/students', icon: 'pi pi-users' },
        { label: 'Logout', icon: 'pi pi-sign-out', command: () => this.onLogout() }
      ];
    } else if (this.user.role === UserRole.STUDENT) {
      this.navLinks = [
        { label: 'Home', routerLink: '/home', icon: 'pi pi-home' },
        { label: 'Profile', routerLink: '/home/profile', icon: 'pi pi-user' },
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

  private async connectToNotificationService() {
    await this.notificationService.connect().then(async () => {
      await this.notificationService.receiveNotification((message: string) => {
        console.log('Notification received:', message);
        this.notificationCount.update(count => count + 1);
      })
    })
    this.destoryRef.onDestroy(() => {
      this.notificationService.stopConnection();
    })
  }
}
