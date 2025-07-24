import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import {User, LoginRequest, LoginResponse, LogoutRequest, RefreshRequest} from '../models/user.model';
import { TokenService } from './token.service';
import {routes} from '../constants/routs';
import {RegisterStudentRequest} from '../models/student.model';
import {UserRole} from '../enums/user-role';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = routes.baseUrl;
  private currentUserSubject = new BehaviorSubject<User | null>(null);
  public readonly currentUser$ = this.currentUserSubject.asObservable();

  constructor(
    private http: HttpClient,
    private tokenService: TokenService)
  {
    this.loadUserFromToken();
  }

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.API_URL + routes.authLogin, credentials)
      .pipe(
        tap(response => {
          this.setSession(response);
        })
      );
  }

  registerStudent(userData: RegisterStudentRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.API_URL + routes.authRegisterStudent, userData);
  }

  logout(): void {
    const refreshToken = this.tokenService.getRefreshToken();
    this.tokenService.clearTokens();
    this.currentUserSubject.next(null);
    this.http.post<LogoutRequest>(this.API_URL + routes.authLogout, refreshToken);
  }

  refreshToken(): Observable<LoginResponse> {
    const refreshToken = this.tokenService.getRefreshToken();
    const accessToken = this.tokenService.getToken();
    const payload: RefreshRequest = { refreshToken, accessToken };
    return this.http.post<LoginResponse>(this.API_URL + routes.authRefresh, payload)
      .pipe(
        tap(response => {
          this.setSession(response);
        })
      );
  }

  isAuthenticated(): boolean {
    const token = this.tokenService.getToken();
    if (!token) {
      return false;
    }

    const refreshToken = this.tokenService.getRefreshToken();
    if (refreshToken && !this.tokenService.isRefreshTokenExpired()) {
      return true;
    }

    return false;
  }

  redirectAfterLogin() {
    if (this.isAdmin()) {
      window.location.href = routes.adminHomePage;
    } else {
      window.location.href = routes.studentHomePage;
    }
  }

  hasRole(role: UserRole): boolean {
     const currentUser = this.currentUserSubject.value;
    if (!currentUser) {
      return false;
    }
     return currentUser.role === role;

  }

  isAdmin(): boolean {
    return this.hasRole(UserRole.ADMIN);
  }

  isStudent(): boolean {
    return this.hasRole(UserRole.STUDENT);
  }

  getToken(): string | null {
    return this.tokenService.getToken();
  }

  getCurrentUser$(): Observable<User | null> {
    return this.currentUser$;
  }

  private setSession(response: LoginResponse): void {
    this.tokenService.setToken(response.accessToken);
    this.tokenService.setRefreshToken(response.refreshToken, response.refreshExpiresAt);

    this.SetUserFromToken();
  }

  private extractUserFromTokenPayload(tokenPayload: any): User {
    return {
      id: (tokenPayload as any)["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"] || '',
      role: (tokenPayload as any)["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || ''
    };
  }

  private SetUserFromToken() {
    const tokenPayload = this.tokenService.decodeToken();
    const user = this.extractUserFromTokenPayload(tokenPayload);
    this.currentUserSubject.next(user);
  }

  private loadUserFromToken(): void {
    if (this.isAuthenticated()) {
      if (this.currentUserSubject.value) {
        return;
      }
      const tokenPayload = this.tokenService.decodeToken();
      if (!tokenPayload) {
        this.clearSession();
        return;
      }

      const user = this.extractUserFromTokenPayload(tokenPayload);
      this.currentUserSubject.next(user);
    }
  }

  private clearSession(): void {
    this.tokenService.clearTokens();
    this.currentUserSubject.next(null);
  }

}
