import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import {tokenInterceptor} from './core/interceptors/token.interceptor';
import {refreshTokenInterceptor} from './core/interceptors/refresh-token.interceptor';
import {errorInterceptor} from './core/interceptors/error.interceptor';
import {MessageService} from 'primeng/api';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    provideHttpClient(
      withInterceptors([
        refreshTokenInterceptor,
        tokenInterceptor,
        errorInterceptor
      ])
    ),
    MessageService
  ]
};
