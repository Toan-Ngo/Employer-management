import { Injectable } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivateChild,
} from '@angular/router';
import { TokenStorageService } from './token-storage.service';
import { UrlConstants } from '../constants/url.constants';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private router: Router,
    private tokenService: TokenStorageService,
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean {
    const requiredPolicy = route.data['requiredPolicy'] as string | undefined;
    const user = this.tokenService.getUser();

    //  Chưa login → redirect login
    if (!user) {
      this.router.navigate([UrlConstants.LOGIN || '/login'], {
        queryParams: { returnUrl: state.url },
      });
      return false;
    }

    //  Nếu route không yêu cầu quyền → cho qua
    if (!requiredPolicy) {
      return true;
    }

    //  Kiểm tra quyền
    const permissions: string[] = user.permissions || [];

    if (permissions.includes(requiredPolicy)) {
      return true;
    }

    //  Không có quyền
    this.router.navigate([UrlConstants.ACCESS_DENIED || '/403'], {
      queryParams: { returnUrl: state.url },
    });
    return false;
  }
  canActivateChild(
    childRoute: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean {
    return this.canActivate(childRoute, state);
  }
}
