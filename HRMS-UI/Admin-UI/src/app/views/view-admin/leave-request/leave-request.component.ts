import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  LeaveRequestApiClient,
  LeaveRequestDto,
} from '../../../api/admin-api.service.generated';

@Component({
  selector: 'app-leave-request',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './leave-request.component.html',
})
export class LeaveRequestComponent implements OnInit {
  leaves: LeaveRequestDto[] = [];
  isLoading = false;

  constructor(
    private leaveApi: LeaveRequestApiClient,
    private cdr: ChangeDetectorRef,
  ) {}

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.isLoading = true;
    this.leaveApi.getLeaveRequests().subscribe({
      next: (res) => {
        this.leaves = res;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Lỗi tải đơn nghỉ:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  approveLeave(id: number | undefined) {
    if (id && confirm('Bạn có chắc chắn muốn DUYỆT đơn xin nghỉ này?')) {
      this.leaveApi.approveLeaveRequest(id).subscribe({
        next: () => {
          alert('Đã duyệt đơn!');
          this.loadData();
        },
        error: (err) => alert('Lỗi khi duyệt: ' + err),
      });
    }
  }

  rejectLeave(id: number | undefined) {
    if (id && confirm('Bạn có chắc chắn muốn TỪ CHỐI đơn xin nghỉ này?')) {
      this.leaveApi.rejectLeaveRequest(id).subscribe({
        next: () => {
          alert('Đã từ chối đơn!');
          this.loadData();
        },
        error: (err) => alert('Lỗi khi từ chối: ' + err),
      });
    }
  }
}
