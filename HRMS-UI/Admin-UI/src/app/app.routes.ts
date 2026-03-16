import { Routes } from '@angular/router';
import { DefaultLayoutComponent } from './layout';

export const routes: Routes = [
  {
    path: '',
    component: DefaultLayoutComponent,
    children: [

      { path: '', redirectTo: 'workforce-overview', pathMatch: 'full' },

      {
        path: 'workforce-overview',
        loadChildren: () => import('./views/workforce-overview/routes').then(m => m.routes)
      },

      {
        path: 'employees',
        loadChildren: () => import('./views/employees/routes').then(m => m.routes)
      },

      {
        path: 'department',
        loadChildren: () => import('./views/department/routes').then(m => m.routes)
      },

      {
        path: 'positions',
        loadChildren: () => import('./views/positions/routes').then(m => m.routes)
      },

      {
        path: 'contracts',
        loadChildren: () => import('./views/contracts/routes').then(m => m.routes)
      },

      {
        path: 'attendance',
        loadChildren: () => import('./views/attendance/routes').then(m => m.routes)
      },

      {
        path: 'leave-request',
        loadChildren: () => import('./views/leave-request/routes').then(m => m.routes)
      },

      {
        path: 'payroll',
        loadChildren: () => import('./views/payroll/routes').then(m => m.routes)
      },

      {
        path: 'reports',
        loadChildren: () => import('./views/reports/routes').then(m => m.routes)
      },

      {
        path: 'settings',
        loadChildren: () => import('./views/settings/routes').then(m => m.routes)
      }

    ]
  }
];