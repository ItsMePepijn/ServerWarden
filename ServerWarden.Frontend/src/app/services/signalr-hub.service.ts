import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { Observable } from 'rxjs';
import { ApiResponse } from '../models/common';

@Injectable({
  providedIn: 'root'
})
export class SignalrHubService {
  private hubConnection: HubConnection;

  public onServerUpdate$: Observable<void>;
  public onServerInstallLog$: Observable<string>;

  constructor() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl("api/serverHub", { accessTokenFactory: () => localStorage.getItem('token') || '' })
      .withAutomaticReconnect()
      .build();
    
    this.onServerUpdate$ = new Observable<void>((observer) => {
      this.hubConnection.on('ServerUpdate', () => {
        observer.next();
      });
    });

    this.onServerInstallLog$ = new Observable<string>((observer) => {
      this.hubConnection.on('ServerInstallLog', (log: string) => {
        observer.next(log);
      });
    });
  }

  public startConnection(): Observable<void> {
    return new Observable<void>((observer) => {
      this.hubConnection
        .start()
        .then(() => {
          console.log('Connection established with SignalR hub');
          observer.next();
          observer.complete();
        })
        .catch((error) => {
          console.error('Error connecting to SignalR hub:', error);
          observer.error(error);
        });
    });
  }

  public async joinServerGroup(serverId: string): Promise<ApiResponse> {
    let result = await this.hubConnection.invoke('joinServerGroup', serverId);

    return result.value as ApiResponse;
  }

  public disconnect(): void {
    this.hubConnection.stop();
  }

  public async connectToServer(serverId: string): Promise<ApiResponse> {
    let result = await this.hubConnection.invoke('connectToServer', serverId);

    return result.value as ApiResponse;
  }
}
