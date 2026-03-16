import { Routes } from '@angular/router';
import { PositionsComponent } from './positions.component';

export const routes: Routes = [
  {
    path: '',
    component: PositionsComponent,
    data: {
      title: 'Position'
    }
  }
];