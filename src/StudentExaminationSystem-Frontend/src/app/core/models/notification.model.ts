export interface NotificationModel {
  id: string;
  message: string;
  timeStamp: Date;
  isRead: boolean;
  type?: 'info' | 'warning' | 'error' | 'success';
}
