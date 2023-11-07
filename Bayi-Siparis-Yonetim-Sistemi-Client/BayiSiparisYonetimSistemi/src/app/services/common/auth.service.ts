import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtHelper: JwtHelperService) { }

  private isAuthenticatedSubject = new BehaviorSubject<boolean>(_isAuthenticated);
  public isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
  identityCheck() {
    const token: string = localStorage.getItem("accessToken");

    //const decodeToken = this.jwtHelper.decodeToken(token);
    //const expirationDate: Date = this.jwtHelper.getTokenExpirationDate(token);
    let expired: boolean;
    try {
      expired = this.jwtHelper.isTokenExpired(token);
    } catch {
      expired = true;
    }

    _isAuthenticated = token != null && !expired;
    this.isAuthenticatedSubject.next(_isAuthenticated);
  }

  get isAuthenticated(): boolean {
    return _isAuthenticated;
  }
  isAdmin(): boolean {
    const token: string = localStorage.getItem("accessToken");
    if (token && !this.jwtHelper.isTokenExpired(token)) {
      const decodedToken = this.jwtHelper.decodeToken(token);
      const roles = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] || decodedToken['role'];
      // Birden fazla rol varsa ve bir dizi olarak saklanıyorsa, 'Admin' rolünü kontrol edin.
      if (Array.isArray(roles)) {
        return roles.includes('Admin');
      } else {
        // Tek bir rol varsa, direkt olarak 'Admin' olup olmadığını kontrol edin.
        return roles === 'Admin';
      }
    }
    return false;
  }
  
  
}
export let _isAuthenticated: boolean;
