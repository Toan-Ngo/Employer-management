import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SettingsService } from '../../../shared/service/settings.service';

@Component({
  selector: 'app-settings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './settings.component.html',
})
export class SettingsComponent implements OnInit {
  // Khởi tạo giá trị mặc định
  settings = {
    textColor: '#212529',
    fontSize: 14,
    isDarkMode: false,
    brightness: 100,
    lineHeight: 1.5,
    borderRadius: 8,
  };

  constructor(private settingsService: SettingsService) {}

  ngOnInit() {
    // Load cài đặt từ service khi vào trang
    const saved = this.settingsService.getSettings();
    if (saved) {
      this.settings = { ...this.settings, ...saved };
    }
  }

  onSettingsChange() {
    this.settingsService.applySettings(this.settings);
  }

  resetSettings() {
    if (confirm('Bạn muốn khôi phục cài đặt gốc?')) {
      this.settings = {
        textColor: '#212529',
        fontSize: 14,
        isDarkMode: false,
        brightness: 100,
        lineHeight: 1.5,
        borderRadius: 8,
      };
      this.onSettingsChange();
    }
  }
}
