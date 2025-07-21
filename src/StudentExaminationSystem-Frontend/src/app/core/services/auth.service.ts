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
    return this.http.post<LoginResponse>(this.API_URL + routes.authRegisterStudent, userData)
      .pipe(
        tap(response => {
          this.setSession(response);
        })
      );
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

    const refreshTokenExpiry = Number(this.tokenService.getRefreshToken());
    if (!isNaN(refreshTokenExpiry) && refreshTokenExpiry > new Date().getTime()) {
      return true;
    }

    return false;
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
    this.tokenService.setRefreshToken(response.refreshToken);

    this.loadUserFromToken();
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

      const user: User = {
        id: tokenPayload.sub,
        role: tokenPayload.role as UserRole
      }

      this.currentUserSubject.next(user);
    }
  }

  private clearSession(): void {
    this.tokenService.clearTokens();
    this.currentUserSubject.next(null);
  }

}
