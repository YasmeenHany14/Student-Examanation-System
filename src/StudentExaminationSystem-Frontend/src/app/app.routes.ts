import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';

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
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'home/subjects',
    loadComponent: () => import('./pages/subjects/subjects-page')
      .then(m => m.SubjectsPage),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'home/difficulty-profiles',
    loadComponent: () => import('./pages/difficulty-profiles/difficulty-profiles')
      .then(m => m.DifficultyProfilesPage),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'home/students',
    loadComponent: () => import('./pages/students/students')
      .then(m => m.Students),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: '',
    redirectTo: 'home/dashboard',
    pathMatch: 'full'
  }
  ]
