import { Injectable } from '@angular/core';
import {TokenPayload} from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
  private readonly ACCESS_TOKEN_KEY = 'access_token';
  private readonly ACCESS_TOKEN_EXPIRY = 'access_token_expiry';
  private readonly REFRESH_TOKEN_KEY = 'refresh_token';
  private readonly REFRESH_TOKEN_EXPIRY_KEY = 'refresh_token_expiry';

  setToken(token: string): void {
    const payload: TokenPayload = JSON.parse(atob(token.split('.')[1]));
    if (!payload || !payload.exp) {
      throw new Error('Invalid token format');
    }
    localStorage.setItem(this.ACCESS_TOKEN_KEY, token);
    localStorage.setItem(this.ACCESS_TOKEN_EXPIRY, new Date(payload.exp * 1000).toISOString());
  }

  getToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  setRefreshToken(refreshToken: string, expiryDate?: string): void {
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refreshToken);
    if (expiryDate) {
      localStorage.setItem(this.REFRESH_TOKEN_EXPIRY_KEY, expiryDate);
    }
  }

  decodeToken(): TokenPayload | null {
    const token = this.getToken();

    if (!token)
      return null;
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      console.log(typeof payload)
      payload.sub
      return payload as TokenPayload;
    } catch (error) {
      console.error('Failed to decode token:', error);
      return null;
    }
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  getRefreshTokenExpiry(): Date | null {
    const expiry = localStorage.getItem(this.REFRESH_TOKEN_EXPIRY_KEY);
    return expiry ? new Date(expiry) : null;
  }

  isRefreshTokenExpired(): boolean {
    const expiry = this.getRefreshTokenExpiry();
    if (!expiry) return true;
    return expiry.getTime() <= Date.now();
  }

  isAccessTokenExpired(): boolean {
    const expiry = this.getTokenExpiry();
    if (!expiry) return true;
    return expiry.getTime() <= Date.now();
  }

  getTokenExpiry(): Date | null {
    const expiry = localStorage.getItem(this.ACCESS_TOKEN_EXPIRY);
    return expiry ? new Date(expiry) : null;
  }

  clearTokens(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_EXPIRY_KEY);
    localStorage.removeItem(this.ACCESS_TOKEN_EXPIRY);
  }
}
