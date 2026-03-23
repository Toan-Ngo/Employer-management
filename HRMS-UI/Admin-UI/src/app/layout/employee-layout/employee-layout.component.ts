import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';
import { NgScrollbar } from 'ngx-scrollbar';
import {
  ContainerComponent,
  ShadowOnScrollDirective,
  SidebarBrandComponent,
  SidebarComponent,
  SidebarFooterComponent,
  SidebarHeaderComponent,
  SidebarNavComponent,
  SidebarToggleDirective,
  SidebarTogglerDirective,
} from '@coreui/angular';

import {
  DefaultFooterComponent,
  DefaultHeaderComponent,
} from '../default-layout';

@Component({
  selector: 'app-employee-layout',
  standalone: true, // Đảm bảo có standalone nếu dự án bạn dùng style này
  templateUrl: './employee-layout.component.html',
  imports: [
    SidebarComponent,
    SidebarHeaderComponent,
    SidebarBrandComponent,
    SidebarNavComponent,
    SidebarFooterComponent,
    SidebarToggleDirective,
    SidebarTogglerDirective,
    ContainerComponent,
    DefaultFooterComponent,
    DefaultHeaderComponent,
    NgScrollbar,
    RouterOutlet,
    RouterLink,
    ShadowOnScrollDirective,
  ],
})
export class EmployeeLayoutComponent {
  public employeeNavItems = [
    {
      name: 'Chấm công cá nhân',
      url: '/portal/attendance',
      iconComponent: { name: 'cil-calendar' },
    },
    {
      name: 'Đơn xin nghỉ phép',
      url: '/portal/leave-request',
      iconComponent: { name: 'cil-notes' },
    },
    {
      name: 'Hồ sơ của tôi',
      url: '/portal/profile',
      iconComponent: { name: 'cil-user' },
    },

    {
      name: 'Cài Đặt',
      url: '/portal/setting',
      iconComponent: { name: 'cilSettings' },
    },
  ];
}
