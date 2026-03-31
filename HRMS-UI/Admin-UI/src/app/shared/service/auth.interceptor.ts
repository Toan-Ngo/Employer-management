import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpClient,
} from '@angular/common/http';
import { Observable, throwError, BehaviorSubject, EMPTY } from 'rxjs';
import { catchError, filter, take, switchMap } from 'rxjs/operators';
import { TokenStorageService } from './token-storage.service';
import { Router } from '@angular/router';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  private isRefreshing = false;
  private refreshTokenSubject: BehaviorSubject<any> = new BehaviorSubject<any>(
    null,
  );
  private isBrowser: boolean;

  constructor(
    private tokenService: TokenStorageService,
    private http: HttpClient,
    private router: Router,
    @Inject(PLATFORM_ID) platformId: Object,
  ) {
    this.isBrowser = isPlatformBrowser(platformId);
  }

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler,
  ): Observable<HttpEvent<any>> {
    if (!this.isBrowser && !request.url.includes('auth/login')) {
      return EMPTY;
    }

    const token = this.tokenService.getToken();
    let authReq = request;

    if (token) {
      authReq = this.addTokenHeader(request, token);
    }

    authReq = authReq.clone({
      withCredentials: true,
    });

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !authReq.url.includes('auth/login')) {
          return this.handle401Error(authReq, next);
        }
        return throwError(() => error);
      }),
    );
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      const token = this.tokenService.getToken();
      const refreshToken = this.tokenService.getRefreshToken();

      if (!refreshToken) {
        this.isRefreshing = false;
        this.tokenService.signOut();
        this.router.navigate(['/login']);
        return throwError(() => new Error('Phiên làm việc hết hạn'));
      }

      const refreshUrl = 'https://localhost:44387/api/admin/auth/refresh-token';

      return this.http
        .post<any>(
          refreshUrl,
          {
            token: token,
            refreshToken: refreshToken,
          },
          { withCredentials: true },
        )
        .pipe(
          switchMap((res: any) => {
            this.isRefreshing = false;

            this.tokenService.saveToken(res.token);
            this.tokenService.saveRefreshToken(res.refreshToken);

            this.refreshTokenSubject.next(res.token);

            return next.handle(this.addTokenHeader(request, res.token));
          }),
          catchError((err) => {
            this.isRefreshing = false;
            this.tokenService.signOut();
            this.router.navigate(['/login']);
            return throwError(() => err);
          }),
        );
    }

    return this.refreshTokenSubject.pipe(
      filter((token) => token !== null),
      take(1),
      switchMap((jwt) => next.handle(this.addTokenHeader(request, jwt))),
    );
  }

  private addTokenHeader(request: HttpRequest<any>, token: string) {
    return request.clone({
      headers: request.headers.set('Authorization', `Bearer ${token}`),
    });
  }
}
