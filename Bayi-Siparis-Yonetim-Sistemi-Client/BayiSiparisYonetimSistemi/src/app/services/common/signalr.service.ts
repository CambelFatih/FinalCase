import { Inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnections: Map<string, HubConnection> = new Map();
  constructor(@Inject("baseSignalRUrl") private baseSignalRUrl: string) { }

  start(hubUrl: string) {
    hubUrl = this.baseSignalRUrl + hubUrl;

    const builder: HubConnectionBuilder = new HubConnectionBuilder();

    const hubConnection: HubConnection = builder
    .withUrl(hubUrl, {
      // accessTokenFactory fonksiyonu ile token'ı sağlayın
      accessTokenFactory: () => localStorage.getItem("accessToken"),
    })
    .withAutomaticReconnect()
    .build();

      hubConnection.start()
      .then(() => {
        console.log("Connected");
        this.hubConnections.set(hubUrl, hubConnection); // Bağlantıyı haritaya ekle.
      })
      .catch(error => setTimeout(() => this.start(hubUrl), 2000));

    hubConnection.onreconnected(connectionId => console.log("Reconnected"));
    hubConnection.onreconnecting(error => console.log("Reconnecting"));
    hubConnection.onclose(error => console.log("Close reconnection"));
    return hubConnection;
  }

  invoke(hubUrl: string, procedureName: string, message: any, successCallBack?: (value) => void, errorCallBack?: (error) => void) {
    this.start(hubUrl).invoke(procedureName, message)
      .then(successCallBack)
      .catch(errorCallBack);
  }

sendMessageToCustomer(hubUrl: string, customerId: string, message: string) {
  this.invoke(hubUrl, 'SendMessageToCustomer', { customerId, message },
    () => console.log("Message sent to customer."),
    (error) => console.log("Failed to send message to customer:", error));
}


  on(hubUrl: string, procedureName: string, callBack: (...message: any) => void) {
    this.start(hubUrl).on(procedureName, callBack);
  }
  
  disconnect(hubUrl: string): void {
    const fullHubUrl = this.baseSignalRUrl + hubUrl;
    const hubConnection = this.hubConnections.get(fullHubUrl);
    if (hubConnection) {
      hubConnection.stop()
        .then(() => console.log(`${hubUrl} connection stopped`))
        .catch(err => console.log(`Error while stopping ${hubUrl} connection: `, err));
      this.hubConnections.delete(fullHubUrl); // Bağlantıyı Map'ten sil
    }
  }
}
