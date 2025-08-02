export interface NotificationModel {
  id: string;
  message: string;
  createdAt: Date;
  isRead: boolean;
  type?: 'info' | 'warning' | 'error' | 'success';
}
