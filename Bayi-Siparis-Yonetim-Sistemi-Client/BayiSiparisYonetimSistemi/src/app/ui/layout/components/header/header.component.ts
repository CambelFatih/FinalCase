import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HubUrls } from 'src/app/constants/hub-urls';
import { AuthService } from 'src/app/services/common/auth.service';
import { ComponentType, DynamicLoadComponentService } from 'src/app/services/common/dynamic-load-component.service';
import { SignalRService } from 'src/app/services/common/signalr.service';
import { CustomToastrService, ToastrMessageType, ToastrPosition } from 'src/app/services/ui/custom-toastr.service';

@Component({
  selector: 'app-uiheader',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {
  title: string="BSYS-Client";
  constructor(public authService: AuthService, private toastrService: CustomToastrService, private router: Router, private dynamicLoadComponentService: DynamicLoadComponentService,private signalRService: SignalRService) { }

  ngOnInit(): void {
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

  loadComponent() {
   // this.dynamicLoadComponentService.loadComponent(ComponentType.BasketsComponent, this.dynamicLoadComponentDirective.viewContainerRef);
  }
}
