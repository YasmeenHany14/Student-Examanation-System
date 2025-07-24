import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'account/login',
    loadComponent: () => import('./pages/login/login')
      .then(m => m.Login),
  },
  {
    path: 'account/register',
    loadComponent: () => import('./pages/register/register')
      .then(m => m.Register),
  },
  {
    path: 'home/dashboard',
    loadComponent: () => import('./pages/dashboard/dashboard')
      .then(m => m.Dashboard),
  },
  {
    path: 'home/subjects',
    loadComponent: () => import('./pages/subjects/subjects-page')
      .then(m => m.SubjectsPage),
  },
  {
    path: '',
    redirectTo: 'account/login',
    pathMatch: 'full'
  }
  ]
