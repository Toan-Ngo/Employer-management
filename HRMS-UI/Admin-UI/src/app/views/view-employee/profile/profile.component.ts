import {
  Component,
  OnInit,
  ElementRef,
  ViewChild,
  ChangeDetectorRef,
  inject,
} from '@angular/core';
import { HttpClient } from '@angular/common/http';
import {
  EmployeeApiClient,
  EmployeeDto,
} from '../../../api/admin-api.service.generated';
import { TokenStorageService } from '../../../shared/service/token-storage.service';
import {
  CardModule,
  GridModule,
  TableModule,
  BadgeModule,
  SpinnerModule,
  ButtonModule,
} from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { IconModule } from '@coreui/icons-angular';

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    CardModule,
    GridModule,
    TableModule,
    BadgeModule,
    SpinnerModule,
    IconModule,
    ButtonModule,
  ],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;

  userInfo?: EmployeeDto;
  loading = true;
  avatarUrl: string = '';

  private readonly baseUrl = 'https://localhost:44387';
  private cdr = inject(ChangeDetectorRef);

  constructor(
    private employeeApi: EmployeeApiClient,
    private tokenService: TokenStorageService,
    private http: HttpClient,
  ) {}

  ngOnInit(): void {
    this.loadProfile();
  }

  loadProfile() {
    this.loading = true;
    const user = this.tokenService.getUser();
    let empCode = user?.employeeCode;

    if (!empCode && user?.accessToken) {
      empCode = this.getEmployeeCodeFromToken(user.accessToken);
    }

    if (empCode) {
      this.employeeApi.getEmployee(empCode).subscribe({
        next: (res) => {
          this.userInfo = res;
          this.updateAvatarDisplay(res);
          this.loading = false;
          this.cdr.detectChanges();
        },
        error: (err) => {
          console.error('Lỗi tải hồ sơ', err);
          this.loading = false;
        },
      });
    } else {
      this.cdr.detectChanges();
      this.loading = false;
    }
  }

  updateAvatarDisplay(res: EmployeeDto) {
    if (res.avatar && res.avatar.trim() !== '') {
      const path = res.avatar.startsWith('/') ? res.avatar : `/${res.avatar}`;
      this.avatarUrl = `${this.baseUrl}${path}`;
    } else {
      this.avatarUrl = `https://ui-avatars.com/api/?name=${encodeURIComponent(res.fullName || 'User')}&background=0D6EFD&color=fff&size=150`;
    }
  }

  handleImageError() {
    this.avatarUrl = `https://ui-avatars.com/api/?name=${encodeURIComponent(this.userInfo?.fullName || 'User')}&background=random&size=150`;
  }

  triggerFileSelect() {
    this.fileInput.nativeElement.click();
  }

  onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file && this.userInfo?.employeeCode) {
      const formData = new FormData();
      formData.append('file', file);

      const uploadUrl = `${this.baseUrl}/api/admin/employee/${this.userInfo.employeeCode}/upload-avatar`;

      this.http.post(uploadUrl, formData).subscribe({
        next: (res: any) => {
          const path = res.url.startsWith('/') ? res.url : `/${res.url}`;
          this.avatarUrl = `${this.baseUrl}${path}`;

          if (this.userInfo) this.userInfo.avatar = res.url;

          alert('Cập nhật ảnh đại diện thành công!');
        },
        error: (err) => {
          console.error('Lỗi upload ảnh', err);
          alert(
            'Không thể lưu ảnh. Hãy đảm bảo thư mục wwwroot hiện hữu ở Backend.',
          );
        },
      });
    }
  }

  getSeniority(user: EmployeeDto | undefined): string {
    if (!user || !user.hireDate) return 'Chưa xác định';

    if (user.isActive === false) {
      return 'Đã thôi việc';
    }

    const start = new Date(user.hireDate);
    const now = new Date();

    let years = now.getFullYear() - start.getFullYear();
    let months = now.getMonth() - start.getMonth();
    let days = now.getDate() - start.getDate();

    if (days < 0) {
      months--;
      const lastMonth = new Date(
        now.getFullYear(),
        now.getMonth(),
        0,
      ).getDate();
      days += lastMonth;
    }

    if (months < 0) {
      years--;
      months += 12;
    }

    let result = [];
    if (years > 0) result.push(`${years} năm`);
    if (months > 0) result.push(`${months} tháng`);
    if (days > 0 || result.length === 0) result.push(`${days} ngày`);

    return result.join(' ');
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
}
