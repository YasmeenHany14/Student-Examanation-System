import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'account/login',
    loadComponent: () => import('./pages/login/login')
      .then(m => m.Login),
  },
  {
    path: '',
    redirectTo: 'account/login',
    pathMatch: 'full'
  }
  ]
