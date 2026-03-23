import { AuthGuard } from './shared/service/auth.guard';
import { DefaultLayoutComponent } from './layout';
import { EmployeeLayoutComponent } from './layout/employee-layout/employee-layout.component';
import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () =>
      import('./views/auth/login/login.component').then(
        (m) => m.LoginComponent,
      ),
    data: { title: 'Đăng nhập' },
  },

  {
    path: '',
    component: DefaultLayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      { path: '', redirectTo: 'workforce-overview', pathMatch: 'full' },
      {
        path: 'workforce-overview',
        loadChildren: () =>
          import('./views/view-admin/workforce-overview/routes').then(
            (m) => m.routes,
          ),
        data: { requiredPolicy: 'Permissions.Account.View' },
      },
      {
        path: 'employees',
        loadChildren: () =>
          import('./views/view-admin/employees/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Employee.View' },
      },
      {
        path: 'department',
        loadChildren: () =>
          import('./views/view-admin/department/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Department.View' },
      },
      {
        path: 'positions',
        loadChildren: () =>
          import('./views/view-admin/positions/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Position.View' },
      },
      {
        path: 'contracts',
        loadChildren: () =>
          import('./views/view-admin/contracts/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Contract.View' },
      },
      {
        path: 'attendance',
        loadChildren: () =>
          import('./views/view-admin/attendance/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Attendance.Update' },
      },
      {
        path: 'leave-request',
        loadChildren: () =>
          import('./views/view-admin/leave-request/routes').then(
            (m) => m.routes,
          ),
        data: { requiredPolicy: 'Permissions.Leave.Approve' },
      },
      {
        path: 'payroll',
        loadChildren: () =>
          import('./views/view-admin/payroll/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Salary.Create' },
      },
      {
        path: 'feedback',
        loadChildren: () =>
          import('./views/view-admin/reports/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Account.View' },
      },
      {
        path: 'profile',
        loadChildren: () =>
          import('./views/view-admin/profile/routes').then((m) => m.routes),
      },
      {
        path: 'settings',
        loadChildren: () =>
          import('./views/view-admin/settings/routes').then((m) => m.routes),
      },
    ],
  },
  {
    path: 'portal',
    component: EmployeeLayoutComponent,
    canActivate: [AuthGuard],
    canActivateChild: [AuthGuard],
    children: [
      { path: '', redirectTo: 'attendance', pathMatch: 'full' },
      {
        path: 'attendance',
        loadChildren: () =>
          import('./views/view-employee/attendance/routes').then(
            (m) => m.routes,
          ),
        data: { requiredPolicy: 'Permissions.Attendance.View' },
      },
      {
        path: 'leave-request',
        loadChildren: () =>
          import('./views/view-employee/leave-request/routes').then(
            (m) => m.routes,
          ),
        data: { requiredPolicy: 'Permissions.Leave.View' },
      },
      {
        path: 'reports',
        loadChildren: () =>
          import('./views/view-employee/reports/routes').then((m) => m.routes),
        data: { requiredPolicy: 'Permissions.Report.View' },
      },
      // {
      //   path: 'payroll',
      //   loadChildren: () => import('./views/view-employee/payroll/routes').then(m => m.routes),
      //   data: { requiredPolicy: 'Permissions.Salary.View' }, // Hoặc policy riêng của User
      // },
      {
        path: 'profile',
        loadChildren: () =>
          import('./views/view-employee/profile/routes').then((m) => m.routes),
      },
      {
        path: 'setting',
        loadChildren: () =>
          import('./views/view-employee/settings/routes').then((m) => m.routes),
      },
    ],
  },

  {
    path: '403',
    loadComponent: () =>
      import('./views/auth/page403/page403.component').then(
        (m) => m.Page403Component,
      ),
  },
  {
    path: '404',
    loadComponent: () =>
      import('./views/auth/page404/page404.component').then(
        (m) => m.Page404Component,
      ),
  },
  {
    path: '500',
    loadComponent: () =>
      import('./views/auth/page500/page500.component').then(
        (m) => m.Page500Component,
      ),
  },
  { path: '**', redirectTo: 'login' },
];
