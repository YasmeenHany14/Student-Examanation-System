import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OverlayBadgeModule } from 'primeng/overlaybadge';
import { ButtonModule } from 'primeng/button';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { BadgeModule } from 'primeng/badge';
import { NotificationService } from '../../core/services/notification.service';
import { Notification } from '../../core/models/notification.model';
import { ListboxModule } from 'primeng/listbox';
import { PopoverModule } from 'primeng/popover';


@Component({
  selector: 'app-notifications',
  standalone: true,
  imports: [
    CommonModule,
    OverlayBadgeModule,
    ButtonModule,
    ScrollPanelModule,
    BadgeModule,
    ListboxModule,
    PopoverModule
  ],
  templateUrl: './notifications.html',
  styleUrl: './notifications.scss'
})
export class Notifications implements OnInit {
  notifications = signal<Notification[]>([]);
  unreadCount = signal<number>(0);

  private notificationService = inject(NotificationService);
  private destroyRef = inject(DestroyRef);

  async ngOnInit(): Promise<void> {
    this.unreadCount.set(0);
    await this.connectToNotificationService();
  }

  private async connectToNotificationService(): Promise<void> {
    try {
      await this.notificationService.connect();

      this.notificationService.receiveNotification((message: string) => {
        this.addNotification(message);
      });

      this.notificationService.handleDisconnects();
    } catch (error) {
      console.error('Error connecting to notification service:', error);
    }
  }

  private addNotification(message: string): void {
    const newNotification: Notification = {
      id: this.generateId(),
      message,
      timestamp: new Date(),
      isRead: false,
      type: 'info'
    };

    this.notifications.update(notifications => [newNotification, ...notifications]);
    this.updateUnreadCount();
  }

  markAsRead(notificationId: string): void {
    this.notifications.update(notifications =>
      notifications.map(notification =>
        notification.id === notificationId
          ? { ...notification, isRead: true }
          : notification
      )
    );
    this.updateUnreadCount();
  }

  markAllAsRead(): void {
    this.notifications.update(notifications =>
      notifications.map(notification => ({ ...notification, isRead: true }))
    );
    this.updateUnreadCount();
  }

  private updateUnreadCount(): void {
    const unread = this.notifications().filter(n => !n.isRead).length;
    this.unreadCount.set(unread);
  }

  private generateId(): string {
    return Date.now().toString(36) + Math.random().toString(36).substr(2);
  }

  formatTimestamp(timestamp: Date): string {
    const now = new Date();
    const diff = now.getTime() - timestamp.getTime();
    const minutes = Math.floor(diff / 60000);
    const hours = Math.floor(diff / 3600000);
    const days = Math.floor(diff / 86400000);

    if (minutes < 1) return 'Just now';
    if (minutes < 60) return `${minutes}m ago`;
    if (hours < 24) return `${hours}h ago`;
    return `${days}d ago`;
  }
}
