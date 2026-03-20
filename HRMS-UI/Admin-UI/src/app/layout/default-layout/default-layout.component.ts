import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { NgScrollbar } from 'ngx-scrollbar';
import {
  ContainerComponent,
  INavData,
  ShadowOnScrollDirective,
  SidebarBrandComponent,
  SidebarComponent,
  SidebarFooterComponent,
  SidebarHeaderComponent,
  SidebarNavComponent,
  SidebarToggleDirective,
  SidebarTogglerDirective,
} from '@coreui/angular';

import { DefaultFooterComponent, DefaultHeaderComponent } from './';
import { navItems } from './_nav';
import { TokenStorageService } from '../../shared/service/token-storage.service'

@Component({
  standalone: true,
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
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
export class DefaultLayoutComponent {
  public navItems: INavData[] = [];

  constructor(private tokenService: TokenStorageService,
    private router: Router
  ) {
  }
  ngOnInit(): void {
    const user = this.tokenService.getUser();
    if (!user) {
      this.router.navigate(['/login']);
      return;
    }
    const permissions = this.tokenService.getUser()?.permissions ?? []; // lấy quyền user
    this.navItems = navItems(permissions);
  }
}