import {
  ADMIN_API_BASE_URL,
  AuthApiClient,
} from './api/admin-api.service.generated';
import { ApplicationConfig } from '@angular/core';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { MessageService } from 'primeng/api';

import {
  provideRouter,
  withEnabledBlockingInitialNavigation,
  withHashLocation,
  withInMemoryScrolling,
  withRouterConfig,
  withViewTransitions,
} from '@angular/router';

import { IconSetService } from '@coreui/icons-angular';
import { routes } from './app.routes';
import { environment } from '../environments/environment';
import { AlertService } from './shared/service/alert.service';

export const appConfig: ApplicationConfig = {
  providers: [
    { provide: ADMIN_API_BASE_URL, useValue: environment.API_URL },

    provideRouter(
      routes,
      withRouterConfig({
        onSameUrlNavigation: 'reload',
      }),
      withInMemoryScrolling({
        scrollPositionRestoration: 'top',
        anchorScrolling: 'enabled',
      }),
      withEnabledBlockingInitialNavigation(),
      withViewTransitions(),
      withHashLocation(),
    ),

    provideAnimationsAsync(),
    provideHttpClient(),

    AuthApiClient,
    IconSetService,
    MessageService,
    AlertService,
  ],
};
