import {UserRole} from '../enums/user-role';
import { BaseRequestModel, BaseResponseModel } from './common/base-model';

export interface User {
  id: string;
  role: UserRole;
}

export interface LoginRequest extends BaseRequestModel {
  email: string;
  password: string;
}

export interface LoginResponse extends BaseResponseModel {
  accessToken: string;
  refreshToken: string;
  refreshExpiresAt: string;
}

export interface LogoutRequest extends BaseRequestModel {
  refreshToken: string;
}

export interface RefreshRequest extends BaseRequestModel {
  accessToken: string | null;
  refreshToken: string | null;
}

export interface TokenPayload {
  sub: string;
  role: string;
  iat: number;
  exp: number;
}
