import { Injectable } from '@angular/core';
import { UserModel } from '../modules/user.model';

const TOKEN_KEY = 'auth-token';
const REFRESH_TOKEN_KEY = 'refresh-token';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {

  constructor() { }

  signOut(): void {
    window.localStorage.clear();
    window.sessionStorage.clear();
  }

  // Lưu token thuần
  public saveToken(token: string): void {
    window.localStorage.setItem(TOKEN_KEY, token);

    const user = this.decodeUserFromToken(token);
    if (user?.id) {
      this.saveUser({ ...user, accessToken: token });
    }
  }

  public getToken(): string | null {
    return window.localStorage.getItem(TOKEN_KEY);
  }

  public saveRefreshToken(token: string): void {
    window.localStorage.setItem(REFRESH_TOKEN_KEY, token);
  }

  public getRefreshToken(): string | null {
    return window.localStorage.getItem(REFRESH_TOKEN_KEY);
  }

  public saveUser(user: any): void {
    window.localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): UserModel | null {
    const userJson = window.localStorage.getItem(USER_KEY);
    if (!userJson) return null;
    return JSON.parse(userJson);
  }

  public getPermissions(): string[] {
    const user = this.getUser();
    if (user?.permissions?.length) {
      return user.permissions; // dùng permissions từ localStorage nếu có
    }

    const token = this.getToken();
    if (!token) return [];

    const payload = this.decodeJwtPayload(token);
    const roleClaim = payload['http://schemas.microsoft.com/ws/2005/05/identity/claims/role'];
    if (!roleClaim) return [];

    return Array.isArray(roleClaim) ? roleClaim : [roleClaim]; // map role claim thành mảng
  }

  // decode payload JWT an toàn (Base64 URL-safe)
  private decodeJwtPayload(token: string): any {
    try {
      const base64Url = token.split('.')[1];
      let base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
      while (base64.length % 4) base64 += '=';
      return JSON.parse(atob(base64));
    } catch (e) {
      console.error('Invalid JWT token', e);
      return {};
    }
  }

  // Optional: decode unicode base64
  private b64DecodeUnicode(str: string): string {
    return decodeURIComponent(
      Array.prototype.map.call(atob(str), (c: string) => {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
      }).join('')
    );
  }

  public decodeUserFromToken(token: string): UserModel | null {
    const payload = this.decodeJwtPayload(token);
    if (!payload) return null;

    return {
      id: payload.id || payload.ID || null,
      email: payload.email || '',
      name: payload.name || '',
      permissions: payload.Permission || payload.permission || [],
      accessToken: token
    } as unknown as UserModel;
  }
}