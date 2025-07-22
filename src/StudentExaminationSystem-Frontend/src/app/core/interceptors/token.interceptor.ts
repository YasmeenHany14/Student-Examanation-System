import {HttpHandlerFn, HttpInterceptorFn, HttpRequest} from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthService } from '../services/auth.service';
import {addTokenToRequest, isNotAuthRequest} from '../../shared/utils/auth.utils';

export const tokenInterceptor: HttpInterceptorFn = (req: HttpRequest<unknown>, next: HttpHandlerFn) => {
  const authService = inject(AuthService);

  if (isNotAuthRequest(req)) {
    return next(req);
  }

   const token = authService.getToken();
  if (!token) {
    return next(req);
  }

  return next(addTokenToRequest(req, token));
};
