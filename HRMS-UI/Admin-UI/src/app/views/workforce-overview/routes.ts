import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./workforce-overview.component').then(
        (m) => m.WorkForceOverviewComponent,
      ),
    data: {
      title: `Tổng Quan`,
    },
  },
];
