import {HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { switchMap, catchError, throwError, from } from 'rxjs';
import {addTokenToRequest, isNotAuthRequest} from '../../shared/utils/auth.utils';
import {TokenService} from '../services/token.service';
import {routes} from '../constants/routs';

let refreshInProgress = false;

export const refreshTokenInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authService = inject(AuthService);
  const tokenService = inject(TokenService);
  const router = inject(Router);

  // Skip refresh logic for auth requests
  if (isNotAuthRequest(req)) {
    return next(req);
  }

  // Check token expiration before making the request
  if (tokenService.isAccessTokenExpired()) {
    if (tokenService.isRefreshTokenExpired()) {
      authService.logout();
      router.navigate([routes.authLogin]);
      return throwError(() => new Error('Session expired'));
    }

    // Access token expired but refresh token still valid
    if (!refreshInProgress) {
      refreshInProgress = true;
      return from(authService.refreshToken()).pipe(
        switchMap(() => {
          refreshInProgress = false;
          return next(req);
        }),
        catchError(err => {
          refreshInProgress = false;
          authService.logout();
          router.navigate([routes.authLogin]);
          return throwError(() => err);
        })
      );
    }
  }

  return next(req);
};
