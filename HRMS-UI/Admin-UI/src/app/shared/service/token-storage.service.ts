import { Injectable } from '@angular/core';

const USER_KEY = 'auth-user';
const REFRESHTOKEN_KEY = 'auth-refreshtoken';

@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  private accessToken: string | null = null;

  constructor() {}

  signOut(): void {
    window.localStorage.clear();
    this.accessToken = null;
  }

  public saveToken(token: string): void {
    this.accessToken = token;

    const user = this.decodeUserFromToken(token);
    if (user) {
      const userToSave = {
        id: user.id,
        email: user.email,
        name: user.name,
        permissions: user.permissions,
        employeeCode: user.employeeCode,
        avatar: user.avater,
      };
      this.saveUser(userToSave);
    }
  }

  public getToken(): string | null {
    return this.accessToken;
  }

  public saveRefreshToken(token: string): void {
    window.localStorage.removeItem(REFRESHTOKEN_KEY);
    window.localStorage.setItem(REFRESHTOKEN_KEY, token);
  }

  public getRefreshToken(): string | null {
    return window.localStorage.getItem(REFRESHTOKEN_KEY);
  }

  public getEmployeeCode(): string | null {
    const token = this.getToken();
    if (!token) {
      const user = this.getUser();
      return user?.employeeCode || null;
    }
    const payload = this.decodeJwtPayload(token);
    return payload.EmployeeCode || payload.employeeCode || null;
  }

  public saveUser(user: any): void {
    window.localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): any | null {
    const userJson = window.localStorage.getItem(USER_KEY);
    if (!userJson) return null;
    return JSON.parse(userJson);
  }

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

  public decodeUserFromToken(token: string): any {
    const payload = this.decodeJwtPayload(token);
    if (!payload) return null;

    return {
      id: payload.Id || payload.id || null,
      email: payload.email || '',
      name: payload.name || '',
      permissions: payload.Permission || payload.permission || [],
      employeeCode: payload.EmployeeCode || payload.employeeCode || null,
    };
  }
}
