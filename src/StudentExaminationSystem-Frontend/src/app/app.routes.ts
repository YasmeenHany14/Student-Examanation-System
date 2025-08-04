import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import {StudentGuard} from './core/guards/student.guard';

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
    path: 'home',
    loadComponent: () => import('./pages/home/home')
      .then(m => m.Home),
    canActivate: [AuthGuard]
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
    path: 'home/exams',
    loadComponent: () => import('./pages/exam-list/exam-list')
      .then(m => m.ExamList),
    canActivate: [AuthGuard]
  },
  {
    path: 'home/take-exam',
    loadComponent: () => import('./pages/take-exam/take-exam')
      .then(m => m.TakeExam),
    canActivate: [AuthGuard]
  },
  {
    path: 'home/exam/:id',
    loadComponent: () => import('./pages/exam/exam')
      .then(m => m.ExamComponent),
    canActivate: [AuthGuard]
  },
  {
    path: 'auth/login',
    redirectTo: 'account/login',
    pathMatch: 'full'
  },
  {
    path: 'home/questions',
    loadComponent: () => import('./pages/questions/questions')
      .then(m => m.QuestionsPage),
    canActivate: [AuthGuard, AdminGuard]
  },
  {
    path: 'home/profile',
    loadComponent: () => import('./pages/profile/profile')
      .then(m => m.Profile),
    canActivate: [AuthGuard, StudentGuard]
  },
  {
    path: 'home/exam-details',
    loadComponent: () => import('./pages/exam-details/exam-details')
      .then(m => m.ExamDetails),
    canActivate: [AuthGuard]
  },
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  }
  ]
