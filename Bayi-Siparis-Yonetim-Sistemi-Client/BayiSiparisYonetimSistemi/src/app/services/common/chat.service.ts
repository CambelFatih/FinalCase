import { Injectable } from '@angular/core';
import { MessageInfo, MessageInfoDetail } from 'src/app/contracts/chat/chat';
import { BehaviorSubject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class ChatService {
  // Kullanıcıların mesajlarını tutan bir sözlük yapısı
  private userMessages: MessageInfoDetail[]=[];
  private allConnectionIds: string[]=[];
  private messageInfoDetailSubject = new BehaviorSubject<MessageInfoDetail[]>([]);
  constructor() {}

  addMessage(messageInfo: MessageInfo) {
    // Önce kullanıcı için MessageInfoDetail nesnesini alın veya oluşturun
    let messageDetail: MessageInfoDetail | undefined = this.userMessages.find(detail => detail.userId === messageInfo.userId);
  
    if (!messageDetail) {
      messageDetail = new MessageInfoDetail();
      messageDetail.userName = messageInfo.userName;
      messageDetail.userId = messageInfo.userId;
      messageDetail.connectionId = messageInfo.connectionId;
      this.userMessages.push(messageDetail);
    }
    // Mesajı dönüştürün ve ekleyin
    messageDetail.message.push(messageInfo.message);
    this.allConnectionIds.push(messageInfo.connectionId);
  }
  getallConnectionIds()
  {
    return this.allConnectionIds;
  }
  getMessageInfoDetailObservable() {
    return this.messageInfoDetailSubject.asObservable();
  }
  getmessageInfoDetail():MessageInfoDetail[]
  {
    return this.userMessages;
  }
}
