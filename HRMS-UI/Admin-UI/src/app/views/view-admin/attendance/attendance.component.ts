import {
  ChangeDetectorRef,
  Component,
  OnInit,
  ChangeDetectionStrategy,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import {
  AttendanceApiClient,
  AttendanceDto,
} from '../../../api/admin-api.service.generated';

@Component({
  selector: 'app-attendance',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './attendance.component.html',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AttendanceComponent implements OnInit {
  attendances: AttendanceDto[] = [];
  isLoading = false;
  isModalOpen = false; // Quản lý trạng thái hiển thị Modal

  // Model cho form Check In thủ công
  checkInEmployeeId: number | null = null;

  constructor(
    private attendanceApi: AttendanceApiClient,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    this.attendanceApi
      .getAttendances()
      .pipe(
        finalize(() => {
          this.isLoading = false;
          this.cdr.detectChanges();
        }),
      )
      .subscribe({
        next: (res) => {
          this.attendances = res;
        },
        error: (err) => {
          console.error('Lỗi khi tải dữ liệu chấm công:', err);
        },
      });
  }

  // --- Logic quản lý Modal ---
  openModal() {
    this.isModalOpen = true;
    this.cdr.detectChanges(); // Trigger update vì đang dùng OnPush
  }

  closeModal() {
    this.isModalOpen = false;
    this.checkInEmployeeId = null; // Reset form khi đóng
    this.cdr.detectChanges();
  }
  // ---------------------------

  submitCheckIn() {
    if (this.checkInEmployeeId) {
      this.attendanceApi.checkIn(this.checkInEmployeeId).subscribe({
        next: () => {
          alert('Check-in thành công!');
          this.closeModal(); // Đóng modal và reset data
          this.loadData();
        },
        error: (err) => {
          console.error('Lỗi Check-in:', err);
          alert(
            'Đã xảy ra lỗi khi Check-in. Vui lòng xem console để biết chi tiết.',
          );
        },
      });
    }
  }

  submitCheckOut(attendanceId: number | undefined) {
    if (attendanceId) {
      this.attendanceApi.checkOut(attendanceId).subscribe({
        next: () => {
          alert('Check-out thành công!');
          this.loadData();
        },
        error: (err) => {
          console.error('Lỗi Check-out:', err);
          alert('Đã xảy ra lỗi khi Check-out.');
        },
      });
    }
  }

  deleteAttendance(id: number | undefined) {
    if (id && confirm('Xóa bản ghi chấm công này?')) {
      this.attendanceApi.delete(id).subscribe({
        next: () => {
          this.loadData();
        },
        error: (err) => {
          console.error('Lỗi khi xóa:', err);
          alert('Đã xảy ra lỗi khi xóa bản ghi.');
        },
      });
    }
  }
}
