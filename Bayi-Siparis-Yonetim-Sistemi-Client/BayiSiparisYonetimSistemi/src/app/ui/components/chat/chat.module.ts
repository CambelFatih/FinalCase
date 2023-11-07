import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChatComponent } from './chat.component';
import { RouterModule } from '@angular/router';
import { MatExpansionModule } from '@angular/material/expansion';



@NgModule({
  declarations: [  
    ChatComponent
  ],
  imports: [
    CommonModule,
    MatExpansionModule,
    RouterModule.forChild([
      { path: "", component: ChatComponent }
    ])
  ]
})
export class ChatModule { }
