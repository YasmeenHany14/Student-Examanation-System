import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import {provideHttpClient, withInterceptors} from '@angular/common/http';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { routes } from './app.routes';
import {tokenInterceptor} from './core/interceptors/token.interceptor';
import {refreshTokenInterceptor} from './core/interceptors/refresh-token.interceptor';
import {errorInterceptor} from './core/interceptors/error.interceptor';
import {MessageService} from 'primeng/api';
import {providePrimeNG} from 'primeng/config';
import Lara from '@primeuix/themes/lara';
import Aura from '@primeuix/themes/aura';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimationsAsync(),
    providePrimeNG({
      theme: {
        preset: Aura,
        options: {
          cssLayer: {
            name: 'primeng',
            order: 'theme, base, primeng'
          },
          darkModeSelector: 'none'
        }
      }
    }),
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
