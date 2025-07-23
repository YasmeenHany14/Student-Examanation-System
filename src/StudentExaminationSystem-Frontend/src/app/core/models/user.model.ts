import {UserRole} from '../enums/user-role';

export interface User {
  id: string;
  role: UserRole;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  accessToken: string;
  refreshToken: string;
  refreshExpiresAt: string;
}

export interface LogoutRequest {
  refreshToken: string;
}

export interface RefreshRequest {
  accessToken: string | null;
  refreshToken: string | null;
}

export interface TokenPayload {
  sub: string;
  role: string;
  iat: number;
  exp: number;
}
