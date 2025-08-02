import { Component, DestroyRef, inject, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OverlayBadgeModule } from 'primeng/overlaybadge';
import { ButtonModule } from 'primeng/button';
import { ScrollPanelModule } from 'primeng/scrollpanel';
import { BadgeModule } from 'primeng/badge';
import { NotificationService } from '../../core/services/notification.service';
import { NotificationModel } from '../../core/models/notification.model';
import { ListboxModule } from 'primeng/listbox';
import { PopoverModule } from 'primeng/popover';
import {BaseResourceParametersModel} from '../../core/models/common/base-resource-parameters.model';
import { ScrollerModule } from 'primeng/scroller';


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
    PopoverModule,
    ScrollerModule
  ],
  templateUrl: './notifications.html',
  styleUrl: './notifications.scss'
})
export class Notifications implements OnInit {
  notifications = signal<NotificationModel[]>([]);
  unreadCount = signal<number>(0);

  private notificationService = inject(NotificationService);
  private destroyRef = inject(DestroyRef);
  private resourceParams: BaseResourceParametersModel = {
    PageNumber: 1,
    PageSize: 200
  }


  async ngOnInit(): Promise<void> {
    this.unreadCount.set(0);
    await this.connectToNotificationService();
  }




  private async connectToNotificationService(): Promise<void> {
    try {
      await this.notificationService.connect();

        await this.notificationService.loadNotificationsFromServer(this.resourceParams);

      const notifications = await this.notificationService.loadNotificationsFromServer(this.resourceParams);
      this.notifications.set(notifications.data);
      this.updateUnreadCount();

      this.notificationService.receiveNotification((message: string) => {
        this.addNotification(message);
      });

      this.notificationService.handleDisconnects();
    } catch (error) {
      console.error('Error connecting to notification service:', error);
    }
  }

  private addNotification(message: string): void {
    const newNotification: NotificationModel = {
      id: this.generateId(),
      message,
      createdAt: new Date(),
      isRead: false,
      type: 'info'
    };

    this.notifications.update(notifications => [newNotification, ...notifications]);
    this.updateUnreadCount();
  }

  markAllAsRead(): void {
    this.notificationService.markAllAsRead();

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

  formatTimestamp(timestamp: Date | string): string {
    if (typeof timestamp === 'string') {
      timestamp = new Date(timestamp);
    }
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
