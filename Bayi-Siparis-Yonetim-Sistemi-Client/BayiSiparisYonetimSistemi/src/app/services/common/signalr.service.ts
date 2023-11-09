import { Inject, Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import {Observable } from 'rxjs';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ReceiveFunctions } from 'src/app/constants/receive-functions';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  private hubConnections: Map<string, HubConnection> = new Map();
  constructor(@Inject("baseSignalRUrl") private baseSignalRUrl: string) { }


getConnection(hubUrl: string): HubConnection {
  return this.hubConnections.get(this.baseSignalRUrl + hubUrl);
}

start(hubUrl: string) {
  hubUrl = this.baseSignalRUrl + hubUrl;

  const hashubConnection = this.hubConnections.get(hubUrl);
  if (hashubConnection && hashubConnection.state === HubConnectionState.Connected) {
    return hashubConnection;
  } else {
    const hubConnection: HubConnection = new HubConnectionBuilder()
    .withUrl(hubUrl, {
      accessTokenFactory: () => {
        // Return your token here. If this process is asynchronous, you should return a Promise.
        return localStorage.getItem("accessToken");
      }
    })
    .withAutomaticReconnect()
    .build();  
    
    hubConnection.start()
    .then(() => {
      console.log("Connected");
      this.hubConnections.set(hubUrl, hubConnection); // Add the connection to the map.
    })
    .catch(error => setTimeout(() => this.start(hubUrl), 2000));

    hubConnection.onreconnected(connectionId => console.log("Reconnected"));
    hubConnection.onreconnecting(error => console.log("Reconnecting"));
    hubConnection.onclose(error => console.log("Close reconnection"));  
    return hubConnection;
  }
}

// SignalR servis içinde
onMessageFromCustomer(hubUrl: string): Observable<{ message: any, userId: any }> {
  const hubConnection = this.hubConnections.get(this.baseSignalRUrl + hubUrl);
  return new Observable(observer => {
    hubConnection.on(ReceiveFunctions.MessageFromAdmin, (message, userId) => {
      observer.next({ message, userId });
    });
  });
}


on(hubUrl: string, procedureName: string, callBack: (...message: any) => void) {
  const hubConnection = this.hubConnections.get(this.baseSignalRUrl + hubUrl);
  if (hubConnection && hubConnection.state === HubConnectionState.Connected) {
    hubConnection.on(procedureName, callBack);
  }
  else
  {
    this.start(hubUrl).on(procedureName, callBack);
  }
}
  invoke(hubUrl: string, procedureName: string, message: any, successCallBack?: (value) => void, errorCallBack?: (error) => void) {
    const hubConnection = this.hubConnections.get(this.baseSignalRUrl + hubUrl);
    
    if (hubConnection && hubConnection.state === HubConnectionState.Connected) {
      hubConnection
      .invoke(procedureName, message)
        .then(successCallBack)
        .catch(errorCallBack);
    } else {
      errorCallBack(new Error("Hub connection is not in the 'Connected' state."));
    }
  }
  invoke2(hubUrl: string, procedureName: string, connectionId:string ,message: any,successCallBack?: (value) => void, errorCallBack?: (error) => void) {
    const hubConnection = this.hubConnections.get(this.baseSignalRUrl + hubUrl);
    
    if (hubConnection && hubConnection.state === HubConnectionState.Connected) {
      hubConnection
      .invoke(procedureName, connectionId, message)
        .then(successCallBack)
        .catch(errorCallBack);
    } else {
      errorCallBack(new Error("Hub connection is not in the 'Connected' state."));
    }
  }

sendMessageToCustomer(hubUrl: string, customerId: string, message: string) {
  this.invoke(hubUrl, 'SendMessageToCustomer', { customerId, message },
    () => console.log("Message sent to customer."),
    (error) => console.log("Failed to send message to customer:", error));
}

// signalr.service.ts
sendMessageToAdmin(hubUrl: string, message: string) {
  const fullHubUrl = this.baseSignalRUrl + hubUrl;
  const hubConnection = this.hubConnections.get(fullHubUrl);
  
  if (hubConnection && hubConnection.state === HubConnectionState.Connected) {
    this.invoke(hubUrl, 'SendMessageToAdmin', message,
      () => console.log("Message sent to admin."),
      (error) => console.log("Failed to send message to admin:", error));
  } else {
    console.log("Connection not in Connected state. Trying to reconnect...");
    // Burada yeniden bağlantı kurmayı deneyebilirsiniz veya kullanıcıya bir hata mesajı gösterebilirsiniz.
  }
}
  
stopConnection(hubUrl: string): void {
  const connection = this.hubConnections.get(this.baseSignalRUrl + hubUrl);
  if (connection && connection.state === HubConnectionState.Connected) {
    connection.stop().then(() => console.log(`Connection to ${hubUrl} stopped`));
  }
}


  disconnect(hubUrl: string): void {
    const fullHubUrl = this.baseSignalRUrl + hubUrl;
    const hubConnection = this.hubConnections.get(fullHubUrl);
    if (hubConnection && hubConnection.state === HubConnectionState.Connected) {
      hubConnection.stop()
        .then(() => console.log(`${hubUrl} connection stopped`))
        .catch(err => console.log(`Error while stopping ${hubUrl} connection: `, err));
      this.hubConnections.delete(fullHubUrl); // Bağlantıyı Map'ten sil
    }
  }
}
