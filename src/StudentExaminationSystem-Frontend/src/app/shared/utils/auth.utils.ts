import {routes} from '../../core/constants/routs';

export function isNotAuthRequest(req: any): boolean {
  const authUrls = [
    routes.authRefresh,
    routes.authLogin,
    routes.authLogout,
    // routes.authRegisterStudent,
    routes.subjectsDropdown,
  ];

  if (req.url.endsWith(routes.authRegisterStudent) && req.method === 'POST') {
    return true;
  }

  return authUrls.some(url => req.url.endsWith(url));
}

export function addTokenToRequest(req: any, token: string) {
  return req.clone({
    headers: req.headers.set('Authorization', `Bearer ${token}`)
  });
}
