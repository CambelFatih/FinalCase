import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { DynamicLoadComponentDirective } from './directives/common/dynamic-load-component.directive';
import { AuthService } from './services/common/auth.service';
import { ComponentType, DynamicLoadComponentService } from './services/common/dynamic-load-component.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from './services/ui/custom-toastr.service';
import { SignalRService } from './services/common/signalr.service';
import { HubUrls } from './constants/hub-urls';
import { ReceiveFunctions } from './constants/receive-functions';
import { SendFunctions } from './constants/send-functions';
import { MessageInfo,MessageInfoDetail } from './contracts/chat/chat';

declare var $: any

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  message: string = '';      // Kullanıcıdan alınacak mesaj
  isAdmin: boolean = false;  // Yönetici olup olmadığını tutacak özellik.
  userNames: string[] = [];
  messageInfoDetail: MessageInfoDetail[] = [];
  messages: string[] = [];
  selectedUserName: string="null"; // Seçili müşterinin name bilgisi
  @ViewChild(DynamicLoadComponentDirective, { static: true })
  dynamicLoadComponentDirective: DynamicLoadComponentDirective;
  title: string="BSYS-Client";
  constructor(private signalRService: SignalRService, public authService: AuthService, private toastrService: CustomToastrService, private router: Router, private dynamicLoadComponentService: DynamicLoadComponentService) {
    authService.identityCheck(); 
    if(this.authService.isAuthenticated)
    {
    }
  }
  ngOnInit(): void {
    //this.authService.identityCheck()
    this.authService.isAuthenticated$.subscribe((isAuthenticated) => {
      console.log("Messages:", this.messages);
      console.log("MessageInfoDetail:", this.messageInfoDetail);
      if (isAuthenticated) {
        // Giriş yapıldıktan sonra yapılacak işlemler...
        this.setupSignalR()
        console.log("Giriş yapıldı");

      } else {
        // Çıkış yapıldıktan sonra yapılacak işlemler...
        console.log("Çıkış yapıldı");
      }
    });
  }

  private setupSignalR() {
    if(!this.authService.isAdmin())
    {
      console.log("log deneme Authenticated Customer");
      this.signalRService.on(HubUrls.ChatHub, ReceiveFunctions.MessageFromAdmin, (receivedMessage: string) => {
        this.messages.push(receivedMessage);
      });
    }
    else{
      console.log("log deneme Authenticated Admin");
      this.signalRService.on(HubUrls.ChatHub, ReceiveFunctions.MessageFromCustomer, (receivedMessage: MessageInfo) => {
        this.messages.push(receivedMessage.message);     
        console.log(this.processMessage(receivedMessage));    
      });
    } 
  }

  processMessage(message: MessageInfo): MessageInfoDetail[] {
    const existingDetail = this.messageInfoDetail.find(
      (detail) => detail.userId === message.userId
    );

    if (existingDetail) {
      existingDetail.message.push(message.userName+" : "+message.message);
      if(this.selectedUserName!=existingDetail.userName)
      existingDetail.messageCount +=1;
    } else {
      const newDetail = new MessageInfoDetail();
      newDetail.userName = message.userName;
      newDetail.userId = message.userId;
      newDetail.connectionId = message.connectionId;
      newDetail.message = [message.userName+" : "+message.message];
      newDetail.messageCount +=1;

      this.messageInfoDetail.push(newDetail);
      this.userNames.push(message.userName);  
    }

    return this.messageInfoDetail;
  }
  adminSendMessageHandleByUserName(name:string,message:string){
    const existingDetail = this.messageInfoDetail.find(
      (detail) => detail.userName === name
    );
    if(existingDetail){
       existingDetail.message.push(message);
    }
  }
  findMessageCountByUserName(name:string):number
  {
    const existingDetail = this.messageInfoDetail.find(
      (detail) => detail.userName === name
    );
    if(existingDetail){
       return existingDetail.messageCount;
    }
    return 0;
  }
  changeMessageCountByUserName(name:string){
    const existingDetail = this.messageInfoDetail.find(
      (detail) => detail.userName === name
    );
    if(existingDetail){
       existingDetail.messageCount=0;
    }
  }
  findConnectionIdByUserName(name:string):string{
    const existingDetail = this.messageInfoDetail.find(
      (detail) => detail.userName === name
    );
    if(existingDetail){
      return existingDetail.connectionId;
    }
    return "";
  }
  
  signOut() {
    this.signalRService.disconnect(HubUrls.ChatHub);
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
    if (this.authService.isAdmin()) {
      // Eğer kullanıcı admin ise, müşteriye mesaj gönder
      console.log(this.findConnectionIdByUserName(this.selectedUserName));
      let connectionId: string=this.findConnectionIdByUserName(this.selectedUserName);
      this.signalRService.invokeAdmin(HubUrls.ChatHub,SendFunctions.MessageToCustomerSendFunction,connectionId, this.message);
      this.adminSendMessageHandleByUserName(this.selectedUserName,"Sen : "+this.message);
      console.log('Message sent successfully');
      this.message = '';
    } else {
      this.signalRService.invokeCustomer(HubUrls.ChatHub, SendFunctions.MessageToAdminSendFunction, this.message,
        () => {
          this.messages.push("Sen : "+this.message);
          console.log('Message sent successfully');
          this.message = '';
        },
        (error) => {
          console.log('Failed to send message:', error);
        });
    }
  }
  selectUser(userName: string) {
    this.selectedUserName=userName;
    console.log(this.selectedUserName)
    this.changeMessageCountByUserName(userName);
  }
  refreshPage() {
    location.reload();
  }
  loadComponent() {
    this.dynamicLoadComponentService.loadComponent(ComponentType.BasketsComponent, this.dynamicLoadComponentDirective.viewContainerRef);
  }
}
