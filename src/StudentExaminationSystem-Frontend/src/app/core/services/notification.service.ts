import { Injectable } from "@angular/core";
import {HttpTransportType, HubConnection, HubConnectionBuilder} from '@microsoft/signalr';
import {routes} from '../constants/routs';

@Injectable({
  providedIn: 'root'
})

export class NotificationService {
  private readonly hubConnection: HubConnection;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(routes.notificationHub, {
        skipNegotiation: true,
        transport: HttpTransportType.WebSockets
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

  async receiveNotification(callback: (message: string) => void): Promise<void> {
    if (this.hubConnection) {
      this.hubConnection.on('ReceiveNotification', (message: string) => {
        console.log('Notification received: ', message);
        callback(message);
      });
    } else {
      console.error('Hub connection is not established.');
    }
  }

  handleDisconnects = () => {
    this.hubConnection.onclose(() => {
      console.log('Connection lost. Attempting to reconnect...');
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
