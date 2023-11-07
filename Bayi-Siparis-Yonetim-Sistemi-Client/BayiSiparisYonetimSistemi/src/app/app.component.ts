import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { DynamicLoadComponentDirective } from './directives/common/dynamic-load-component.directive';
import { AuthService } from './services/common/auth.service';
import { ComponentType, DynamicLoadComponentService } from './services/common/dynamic-load-component.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';
import * as bootstrap from 'bootstrap'
import { SignalRService } from './services/common/signalr.service';
import { HubUrls } from './constants/hub-urls';
import * as signalR from '@microsoft/signalr';
import { ReceiveFunctions } from './constants/receive-functions';
import { AlertifyService, MessageType, Position } from './services/admin/alertify.service';
import { SendFunctions } from './constants/send-functions';

declare var $: any

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  message: string = '';      // Kullanıcıdan alınacak mesaj
  messages: string[] = [];   // Alınan mesajlar listesi
  selectedCustomerId: string; // Seçili müşterinin ID'si
  @ViewChild(DynamicLoadComponentDirective, { static: true })
  dynamicLoadComponentDirective: DynamicLoadComponentDirective;
  title: string="BSYS-Client";

  constructor(private alertify: AlertifyService,private signalRService: SignalRService, public authService: AuthService, private toastrService: CustomToastrService, private router: Router, private dynamicLoadComponentService: DynamicLoadComponentService) {
    authService.identityCheck();
  }
  ngOnInit(): void {
    // Server'dan gelen mesajları dinle
    // this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
    //   if (isAuthenticated) {
    //     // Kullanıcı giriş yaptığında mesajları dinlemeye başla
    //     this.signalRService.on(HubUrls.AdminHub, 'ReceiveMessage', (receivedMessage: string) => {
    //       this.messages.push(receivedMessage);
    //     });
    //   } else {
    //     // Kullanıcı çıkış yaptığında veya henüz giriş yapmadıysa mesaj dinlemeyi durdur
    //     // Bu kısım SignalRService içinde bir metot olmalıdır, örneğin:
    //    // this.signalRService.stopConnection(HubUrls.AdminHub);
    //   }
    // });
  }

  signOut() {
    this.signalRService.disconnect(HubUrls.AdminHub);
    localStorage.removeItem("accessToken");
    this.authService.identityCheck();
    this.router.navigate([""]);
    this.toastrService.message("Oturum kapatılmıştır!", "Oturum Kapatıldı", {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopRight
    });
  }
  sendMessage() {
    if (!this.authService.isAuthenticated) {
      console.log('Kullanıcı giriş yapmamış.');
      // Kullanıcıya bir hata mesajı gösterin veya giriş yapmasını isteyin.
      this.toastrService.message('Lütfen mesaj göndermek için giriş yapın.', 'Giriş Yapılmamış', {
        messageType: ToastrMessageType.Warning,
        position: ToastrPosition.TopRight
      });
      return;
    }
    // const hubConnection = this.signalRService.getConnection(HubUrls.AdminHub);
    // if (hubConnection.state !== signalR.HubConnectionState.Connected) {
    //   console.log('Bağlantı kurulamadı. Mesaj gönderilemiyor.');
    //   // Bağlantıyı yeniden kurma girişimi yapabilir veya kullanıcıyı bilgilendirebilirsiniz.
    //   return;
    // }
    if (this.authService.isAdmin()) {
      // Eğer kullanıcı admin ise, müşteriye mesaj gönder
      this.signalRService.sendMessageToCustomer(HubUrls.AdminHub, this.selectedCustomerId, this.message);
    } else {
            this.toastrService.message('User veya Bayi Mesaj Gönderdi', 'Mesaj gönderildi', {
        messageType: ToastrMessageType.Info,
        position: ToastrPosition.TopRight
      });
      // Eğer kullanıcı admin değilse, admin'e mesaj gönder
      // this.signalRService.on(HubUrls.ProductHub, ReceiveFunctions.SendMessageToAdmin, message => {
      //   this.alertify.message(message, {
      //     messageType: MessageType.Notify,
      //     position: Position.TopRight
      //   })
      // });
      this.signalRService.invoke(HubUrls.AdminHub, SendFunctions.MessageToAdminSendFunction, this.message,
        () => {
          console.log('Message sent successfully');
          this.message = '';
        },
        (error) => {
          console.log('Failed to send message:', error);
        });
    }
  }
    // Müşteri seçildiğinde bu fonksiyon çağrılacak
    onCustomerSelected(customerId: string) {
      this.selectedCustomerId = customerId;
    }
  loadComponent() {
    this.dynamicLoadComponentService.loadComponent(ComponentType.BasketsComponent, this.dynamicLoadComponentDirective.viewContainerRef);
  }
}
