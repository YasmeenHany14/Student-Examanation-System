export interface Notification {
  id: string;
  message: string;
  timestamp: Date;
  isRead: boolean;
  type?: 'info' | 'warning' | 'error' | 'success';
}

