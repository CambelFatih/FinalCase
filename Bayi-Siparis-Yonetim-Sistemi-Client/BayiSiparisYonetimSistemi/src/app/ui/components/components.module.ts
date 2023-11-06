import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductsModule } from './products/products.module';
import { HomeModule } from './home/home.module';
import { OrderModule } from './order/order.module';
import { BasketsModule } from './baskets/baskets.module';
import { RegisterComponent } from './register/register.component';
import { RegisterModule } from './register/register.module';
import { LoginComponent } from './login/login.component';
import { LoginModule } from './login/login.module';
import { PasswordResetComponent } from './password-reset/password-reset.component';
import { UpdatePasswordComponent } from './update-password/update-password.component';
import { PasswordResetModule } from './password-reset/password-reset.module';
import { UpdatePasswordModule } from './update-password/update-password.module';
import { ChatComponent } from './chat/chat.component';

@NgModule({
  declarations: [
  
    ChatComponent
  ],
  imports: [
    CommonModule,
    ProductsModule,
    OrderModule,
    HomeModule,
    BasketsModule,
    RegisterModule,
    //LoginModule,
    PasswordResetModule,
    UpdatePasswordModule
  ],
  exports: [
    BasketsModule
  ]
})
export class ComponentsModule { }
