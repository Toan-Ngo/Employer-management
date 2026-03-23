import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  private readonly STORAGE_KEY = 'hrms_user_settings';

  getSettings() {
    const data = localStorage.getItem(this.STORAGE_KEY);
    return data ? JSON.parse(data) : null;
  }

  applySettings(s: any) {
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(s));

    const root = document.documentElement;

    // 1. Cỡ chữ & Dòng
    root.style.fontSize = `${s.fontSize}px`;
    root.style.setProperty('--bs-body-line-height', s.lineHeight);

    // 2. Màu sắc & Dark mode
    root.style.setProperty('--bs-body-color', s.textColor);
    root.style.setProperty('--app-text-color', s.textColor);

    if (s.isDarkMode) {
      root.setAttribute('data-bs-theme', 'dark');
      root.style.setProperty('--bs-body-bg', '#121212');
    } else {
      root.setAttribute('data-bs-theme', 'light');
      root.style.setProperty('--bs-body-bg', '#f8f9fa');
    }

    // 3. Độ sáng (Dùng filter lên thẻ html hoặc body)
    root.style.filter = `brightness(${s.brightness}%)`;

    // 4. Bo góc (Ghi đè biến của Bootstrap)
    root.style.setProperty('--bs-border-radius', `${s.borderRadius}px`);
    root.style.setProperty('--bs-card-border-radius', `${s.borderRadius}px`);
  }
}
