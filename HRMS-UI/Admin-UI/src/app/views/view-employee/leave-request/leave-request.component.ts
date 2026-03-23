import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import {
  LeaveRequestApiClient,
  LeaveRequestDto,
  CreateLeaveRequestDto,
  UpdateLeaveRequestDto,
} from '../../../api/admin-api.service.generated';
import { TokenStorageService } from '../../../shared/service/token-storage.service';
import {
  CardModule,
  TableModule,
  ButtonModule,
  BadgeModule,
  ModalModule,
  FormModule,
  GridModule,
} from '@coreui/angular';
import { CommonModule } from '@angular/common';
import { IconModule } from '@coreui/icons-angular';
import { Router } from '@angular/router';

@Component({
  selector: 'app-leave-request',
  standalone: true,
  imports: [
    CommonModule,
    CardModule,
    TableModule,
    ButtonModule,
    BadgeModule,
    ModalModule,
    FormModule,
    ReactiveFormsModule,
    GridModule,
    IconModule,
  ],
  templateUrl: './leave-request.component.html',
})
export class LeaveRequestComponent implements OnInit {
  leaves: LeaveRequestDto[] = [];
  leaveForm: FormGroup;
  visible = false;
  isEdit = false;
  selectedId?: number;
  loading = false;

  private cdr = inject(ChangeDetectorRef);

  constructor(
    private leaveApi: LeaveRequestApiClient,
    private tokenService: TokenStorageService,
    private fb: FormBuilder,
    private router: Router,
  ) {
    this.leaveForm = this.fb.group({
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      reason: ['', [Validators.required, Validators.minLength(5)]],
    });
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    const empCode = this.tokenService.getEmployeeCode();

    if (!empCode) {
      const user = this.tokenService.getUser();
      if (user) {
        console.warn('Đang đợi khôi phục Token RAM...');
        setTimeout(() => this.loadData(), 1000);
      } else {
        this.router.navigate(['/login']);
      }
      return;
    }

    this.leaveApi.getLeaveRequestsByEmployee(empCode).subscribe({
      next: (res) => {
        this.leaves = res;
        this.cdr.detectChanges();
      },
      error: (err) => console.error('Lỗi tải dữ liệu:', err),
    });
  }

  showModalAdd() {
    this.isEdit = false;
    this.selectedId = undefined;
    this.leaveForm.reset();
    this.visible = true;
  }

  showModalEdit(item: LeaveRequestDto) {
    this.isEdit = true;
    this.selectedId = item.id;
    this.leaveForm.patchValue({
      startDate: this.formatDateForInput(item.startDate),
      endDate: this.formatDateForInput(item.endDate),
      reason: item.reason,
    });
    this.visible = true;
  }

  // HÀM QUAN TRỌNG: Sửa lỗi tạo đơn
  saveLeave() {
    if (this.leaveForm.invalid || this.loading) return;

    const empCode = this.tokenService.getEmployeeCode();
    if (!empCode) {
      alert('Phiên làm việc đang được khôi phục, vui lòng thử lại sau 2 giây!');
      this.loadData();
      return;
    }

    this.loading = true;

    // Chuyển đổi ngày để tránh lỗi lệch múi giờ UTC
    const toDate = (dateStr: string) => {
      const d = new Date(dateStr);
      d.setHours(0, 0, 0, 0);
      return d;
    };

    if (this.isEdit && this.selectedId) {
      const updateDto = new UpdateLeaveRequestDto();
      updateDto.startDate = toDate(this.leaveForm.value.startDate);
      updateDto.endDate = toDate(this.leaveForm.value.endDate);
      updateDto.reason = this.leaveForm.value.reason;

      this.leaveApi.updateLeaveRequest(this.selectedId, updateDto).subscribe({
        next: () => this.handleSuccess('Cập nhật đơn thành công'),
        error: (err) => this.handleError(err),
      });
    } else {
      const createDto = new CreateLeaveRequestDto();
      createDto.employeeCode = empCode;
      createDto.startDate = toDate(this.leaveForm.value.startDate);
      createDto.endDate = toDate(this.leaveForm.value.endDate);
      createDto.reason = this.leaveForm.value.reason;

      this.leaveApi.createLeaveRequest(createDto).subscribe({
        next: () => this.handleSuccess('Gửi đơn thành công'),
        error: (err) => this.handleError(err),
      });
    }
  }

  deleteLeave(id: number) {
    if (confirm('Bạn có chắc chắn muốn xóa đơn này?')) {
      this.leaveApi.deleteLeaveRequest(id).subscribe({
        next: () => {
          alert('Xóa thành công');
          this.loadData();
        },
        error: (err) => alert('Không thể xóa đơn này'),
      });
    }
  }

  private handleSuccess(msg: string) {
    alert(msg);
    this.visible = false;
    this.loading = false;
    this.loadData();
  }

  private handleError(err: any) {
    this.loading = false;
    alert('Lỗi: Server không phản hồi hoặc dữ liệu không hợp lệ.');
    console.error('Chi tiết lỗi Server:', err);
  }

  getStatusColor(status?: string | number) {
    const s = status?.toString().toLowerCase();
    if (s === 'approved' || s === '1') return 'success';
    if (s === 'rejected' || s === '2') return 'danger';
    return 'warning';
  }

  private formatDateForInput(date?: Date | string) {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }
}
