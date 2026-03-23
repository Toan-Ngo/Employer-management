import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  PositionApiClient,
  PositionDto,
} from '../../../api/admin-api.service.generated';
import {
  GridModule,
  CardModule,
  ButtonModule,
  BadgeModule,
  TableModule,
  ModalModule,
  FormModule,
  SpinnerModule,
} from '@coreui/angular';

@Component({
  standalone: true,
  selector: 'app-positions',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GridModule,
    CardModule,
    ButtonModule,
    BadgeModule,
    TableModule,
    ModalModule,
    FormModule,
    SpinnerModule,
  ],
  templateUrl: './positions.component.html',
})
export class PositionsComponent implements OnInit {
  positions: PositionDto[] = [];
  isLoading = false;
  visible = false;
  isEditMode = false;
  currentId: number | null = null;
  posForm!: FormGroup;

  private cdr = inject(ChangeDetectorRef);
  private posApi = inject(PositionApiClient);
  private fb = inject(FormBuilder);

  ngOnInit() {
    this.posForm = this.fb.group({
      positionName: ['', [Validators.required, Validators.minLength(2)]],
    });
    this.loadPositions();
  }

  loadPositions() {
    this.isLoading = true;
    this.cdr.detectChanges(); // Hiện Spinner ngay lập tức

    this.posApi.getPositions().subscribe({
      next: (data) => {
        this.positions = data;
        this.isLoading = false;
        this.cdr.detectChanges(); // 3. Ép giao diện vẽ lại danh sách và ẩn Spinner
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges(); // Ẩn Spinner kể cả khi lỗi
      },
    });
  }

  openModal(pos?: PositionDto) {
    this.isEditMode = !!pos;
    this.currentId = pos?.id || null;
    this.posForm.reset({ positionName: pos?.positionName || '' });
    this.visible = true;
  }

  onSubmit() {
    if (this.posForm.invalid) return;
    const name = this.posForm.value.positionName;
    this.isLoading = true;

    if (this.isEditMode && this.currentId) {
      // 1. Tạo DTO để gửi đi (Đảm bảo khớp với yêu cầu của Backend)
      const updateDto = new PositionDto();
      updateDto.id = this.currentId;
      updateDto.positionName = name;

      // 2. Gọi API với ID trên URL và DTO trong Body
      this.posApi.updatePosition(this.currentId, updateDto).subscribe({
        next: () => {
          alert('Cập nhật chức vụ thành công!');
          this.handleSuccess();
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Lỗi cập nhật:', err);
          alert('Cập nhật thất bại. Vui lòng kiểm tra lại!');
        },
      });
    } else {
      // Tạo mới (Phần này thường đã chạy đúng)
      const dto = new PositionDto();
      dto.positionName = name;
      this.posApi.createPosition(dto).subscribe({
        next: () => {
          alert('Tạo chức vụ mới thành công!');
          this.handleSuccess();
        },
        error: () => {
          this.isLoading = false;
          alert('Tạo mới thất bại!');
        },
      });
    }
  }

  deletePos(id: number) {
    if (
      confirm(
        'Xóa chức vụ này? (Chỉ xóa được nếu không có nhân viên nào giữ chức vụ này)',
      )
    ) {
      this.posApi.deletePosition(id).subscribe({
        next: () => this.loadPositions(),
        error: () => alert('Không thể xóa chức vụ đang có nhân sự!'),
      });
    }
  }

  handleSuccess() {
    this.visible = false;
    this.loadPositions();
  }
}
