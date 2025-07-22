import {routes} from '../../core/constants/routs';

export function isNotAuthRequest(req: any): boolean {
  const authUrls = [
    routes.authRefresh,
    routes.authLogin,
    routes.authLogout
  ];
  return authUrls.some(url => req.url.includes(url));
}

export function addTokenToRequest(req: any, token: string) {
  return req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`)
  });
}
