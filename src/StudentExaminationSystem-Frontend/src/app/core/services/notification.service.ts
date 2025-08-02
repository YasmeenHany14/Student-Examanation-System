import {inject, Injectable} from "@angular/core";
import {HttpTransportType, HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {routes} from '../constants/routs';
import {TokenService} from './token.service';
import {PagedListModel} from '../models/common/paged-list.model';
import {NotificationModel} from '../models/notification.model';
import {BaseResourceParametersModel} from '../models/common/base-resource-parameters.model';

@Injectable({
  providedIn: 'root'
})

export class NotificationService {
  private readonly hubConnection: HubConnection;
  private readonly tokenService = inject(TokenService);

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(routes.notificationHub, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets,
        withCredentials: true,
        accessTokenFactory: () => {
          return this.tokenService.getToken() ?? '';
        }
      })
      .withAutomaticReconnect()
      .build();
  }

  getHubConnection(): HubConnection {
    return this.hubConnection;
  }

  async connect(): Promise<void> {
    try {
      await this.hubConnection.start();
      console.log('Notification hub connection established');
    } catch (error) {
      console.error('Error while establishing notification hub connection: ', error);
    }
  }

  receiveNotification(callback: (message: string) => void): void {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveNotification', (message: string) => {
        console.log('Notification received: ', message);
        callback(message);
      });
    } else {
      console.error('Hub connection is not established.');
    }
  }

  // async NotificationsLoaded(callback: (notifications: PagedListModel<NotificationModel>) => void ) {
  //   if (this.hubConnection) {
  //     this.hubConnection.on('NotificationsLoaded', (notifications: PagedListModel<NotificationModel>) => {
  //       console.log('Notifications loaded');
  //       callback(notifications);
  //     });
  //   } else {
  //     console.error('Hub connection is not established.');
  //   }
  // }

  async loadNotificationsFromServer(
    resourceParams: BaseResourceParametersModel
  ): Promise<PagedListModel<NotificationModel>> {
    if (!this.hubConnection) {
      throw new Error('Hub connection not established');
    }

    // Set up listener FIRST
    const notificationsPromise = new Promise<PagedListModel<NotificationModel>>((resolve) => {
      this.hubConnection.on('NotificationsLoaded', resolve);
    });

    // Then trigger the server request
    await this.hubConnection.invoke('LoadNotificationsAsync', resourceParams);

    // Wait for the server's response
    return await notificationsPromise;
  }

  markAllAsRead(): void {
    if (this.hubConnection) {
      this.hubConnection.invoke('MarkNotificationAsReadAsync');
    }
  }

  handleDisconnects = () => {
    this.hubConnection.onclose(() => {
      setTimeout(() => this.connect(), 3000);  // Try reconnecting after 3 seconds
    });
  }

  stopConnection() {
    if (this.hubConnection) {
      this.hubConnection.stop()
        .then(() => console.log('Notification hub connection stopped'))
        .catch(err => console.error('Error while stopping notification hub connection: ', err));
    } else {
      console.error('Hub connection is not established.');
    }
  }

}
