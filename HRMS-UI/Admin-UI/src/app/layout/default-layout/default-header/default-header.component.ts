import { NgTemplateOutlet } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  computed,
  inject,
  input,
  OnInit,
} from '@angular/core';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';

import {
  AvatarComponent,
  BreadcrumbRouterComponent,
  ColorModeService,
  ContainerComponent,
  DropdownComponent,
  DropdownDividerDirective,
  DropdownHeaderDirective,
  DropdownItemDirective,
  DropdownMenuDirective,
  DropdownToggleDirective,
  HeaderComponent,
  HeaderNavComponent,
  HeaderTogglerDirective,
  NavItemComponent,
  NavLinkDirective,
  SidebarToggleDirective,
} from '@coreui/angular';

import { IconDirective } from '@coreui/icons-angular';
import { TokenStorageService } from '../../../shared/service/token-storage.service';
import { UrlConstants } from '../../../shared/constants/url.constants';
import { EmployeeApiClient } from '../../../api/admin-api.service.generated';

@Component({
  selector: 'app-default-header',
  templateUrl: './default-header.component.html',
  styleUrls: ['./default-header.component.scss'],
  standalone: true,
  imports: [
    ContainerComponent,
    HeaderTogglerDirective,
    SidebarToggleDirective,
    IconDirective,
    HeaderNavComponent,
    NavItemComponent,
    NavLinkDirective,
    RouterLink,
    RouterLinkActive,
    NgTemplateOutlet,
    BreadcrumbRouterComponent,
    DropdownComponent,
    DropdownToggleDirective,
    AvatarComponent,
    DropdownMenuDirective,
    DropdownHeaderDirective,
    DropdownItemDirective,
    DropdownDividerDirective,
  ],
})
export class DefaultHeaderComponent extends HeaderComponent implements OnInit {
  readonly #colorModeService = inject(ColorModeService);
  readonly colorMode = this.#colorModeService.colorMode;

  userAvatarSrc: string = '';
  userFullName: string = '';

  private readonly baseUrl = 'https://localhost:44387';
  private cdr = inject(ChangeDetectorRef);

  readonly colorModes = [
    { name: 'light', text: 'Sáng', icon: 'cilSun' },
    { name: 'dark', text: 'Tối', icon: 'cilMoon' },
    { name: 'auto', text: 'Hệ thống', icon: 'cilContrast' },
  ];

  readonly icons = computed(() => {
    const currentMode = this.colorMode();
    return (
      this.colorModes.find((mode) => mode.name === currentMode)?.icon ??
      'cilSun'
    );
  });

  constructor(
    private tokenService: TokenStorageService,
    private router: Router,
    private employeeApi: EmployeeApiClient,
  ) {
    super();
  }

  ngOnInit(): void {
    this.loadUserAvatar();
  }

  loadUserAvatar() {
    const user = this.tokenService.getUser();
    let empCode = user?.employeeCode;

    if (!empCode && user?.accessToken) {
      empCode = this.getEmployeeCodeFromToken(user.accessToken);
    }

    if (empCode) {
      this.employeeApi.getEmployee(empCode).subscribe({
        next: (res) => {
          this.userFullName = res.fullName || 'Người dùng';

          if (res.avatar && res.avatar.trim() !== '') {
            const path = res.avatar.startsWith('/')
              ? res.avatar
              : `/${res.avatar}`;
            this.userAvatarSrc = `${this.baseUrl}${path}`;
          } else {
            this.userAvatarSrc = `https://ui-avatars.com/api/?name=${encodeURIComponent(res.fullName || 'User')}&background=0D6EFD&color=fff&size=40`;
          }

          this.cdr.detectChanges();
        },
        error: () => this.handleImageError(),
      });
    } else {
      this.handleImageError();
    }
  }

  handleImageError() {
    this.userAvatarSrc = `https://ui-avatars.com/api/?name=User&background=random&size=40`;
    this.userFullName = 'Người dùng';

    this.cdr.detectChanges();
  }

  private getEmployeeCodeFromToken(token: string): string | undefined {
    try {
      const base64Url = token.split('.')[1];
      const jsonPayload = decodeURIComponent(
        atob(base64Url.replace(/-/g, '+').replace(/_/g, '/'))
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join(''),
      );
      const decoded = JSON.parse(jsonPayload);
      return decoded.EmployeeCode || decoded.employeeCode;
    } catch {
      return undefined;
    }
  }

  sidebarId = input('sidebar1');

  logout() {
    this.tokenService.signOut();
    this.router.navigate([UrlConstants.LOGIN]);
  }
}
