import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./payroll.component').then(m => m.PayrollComponent),
    data: {
      title: 'Payroll'
    }
  }
];