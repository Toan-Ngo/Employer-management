import {
  AccountApiClient,
  ADMIN_API_BASE_URL,
  AttendanceApiClient,
  AuthApiClient,
  ContractApiClient,
  DepartmentApiClient,
  EmployeeApiClient,
  FeedbackApiClient,
  LeaveRequestApiClient,
  PositionApiClient,
  SalaryApiClient,
} from './api/admin-api.service.generated';
import { ApplicationConfig } from '@angular/core';
import {
  HTTP_INTERCEPTORS,
  provideHttpClient,
  withInterceptorsFromDi,
} from '@angular/common/http';
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

import { provideAnimations } from '@angular/platform-browser/animations';
import { TokenStorageService } from './shared/service/token-storage.service';
import { ToastModule } from '@coreui/angular';
import { AuthInterceptor } from './shared/service/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    { provide: ADMIN_API_BASE_URL, useValue: environment.API_URL },
    provideAnimations(),

    EmployeeApiClient,
    DepartmentApiClient,
    LeaveRequestApiClient,
    SalaryApiClient,
    AttendanceApiClient,
    AuthApiClient,
    ContractApiClient,
    PositionApiClient,
    AccountApiClient,
    FeedbackApiClient,

    provideHttpClient(withInterceptorsFromDi()),
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true,
    },

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

    IconSetService,
    MessageService,
    AlertService,
    ToastModule,
    TokenStorageService,
  ],
};
