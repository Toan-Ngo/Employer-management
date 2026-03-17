import { Routes } from '@angular/router';
import { DefaultLayoutComponent } from './layout';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'workforce-overview',
    pathMatch: 'full',
  },
  {
    path: '',
    loadComponent: () =>
      import('./layout').then((m) => m.DefaultLayoutComponent),
    data: {
      title: 'Home',
    },
    children: [
      {
        path: 'workforce-overview',
        loadChildren: () =>
          import('./views/workforce-overview/routes').then((m) => m.routes),
      },

      {
        path: 'employees',
        loadChildren: () =>
          import('./views/employees/routes').then((m) => m.routes),
      },

      {
        path: 'department',
        loadChildren: () =>
          import('./views/department/routes').then((m) => m.routes),
      },

      {
        path: 'positions',
        loadChildren: () =>
          import('./views/positions/routes').then((m) => m.routes),
      },

      {
        path: 'contracts',
        loadChildren: () =>
          import('./views/contracts/routes').then((m) => m.routes),
      },

      {
        path: 'attendance',
        loadChildren: () =>
          import('./views/attendance/routes').then((m) => m.routes),
      },

      {
        path: 'leave-request',
        loadChildren: () =>
          import('./views/leave-request/routes').then((m) => m.routes),
      },

      {
        path: 'payroll',
        loadChildren: () =>
          import('./views/payroll/routes').then((m) => m.routes),
      },

      {
        path: 'reports',
        loadChildren: () =>
          import('./views/reports/routes').then((m) => m.routes),
      },

      {
        path: 'settings',
        loadChildren: () =>
          import('./views/settings/routes').then((m) => m.routes),
      },
    ],
  },
  {
    path: '404',
    loadComponent: () =>
      import('./views/auth/page404/page404.component').then(
        (m) => m.Page404Component,
      ),
    data: {
      title: 'Page 404',
    },
  },
  {
    path: '500',
    loadComponent: () =>
      import('./views/auth/page500/page500.component').then(
        (m) => m.Page500Component,
      ),
    data: {
      title: 'Page 500',
    },
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./views/auth/login/login.component').then(
        (m) => m.LoginComponent,
      ),
    data: {
      title: 'Login Page',
    },
  },
  {
    path: 'register',
    loadComponent: () =>
      import('./views/auth/register/register.component').then(
        (m) => m.RegisterComponent,
      ),
    data: {
      title: 'Register Page',
    },
  },
  { path: '**', redirectTo: 'workforce-overview' },
];
