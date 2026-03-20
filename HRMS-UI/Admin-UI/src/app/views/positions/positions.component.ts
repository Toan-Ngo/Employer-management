import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { PositionApiClient, PositionDto } from '../../api/admin-api.service.generated';
import {
  GridModule, CardModule, ButtonModule, BadgeModule, TableModule,
  ModalModule, FormModule, SpinnerModule
} from '@coreui/angular';

@Component({
  standalone: true,
  selector: 'app-positions',
  imports: [
    CommonModule, ReactiveFormsModule, GridModule, CardModule, ButtonModule,
    BadgeModule, TableModule, ModalModule, FormModule, SpinnerModule
  ],
  templateUrl: './positions.component.html'
})
export class PositionsComponent implements OnInit {
  positions: PositionDto[] = [];
  isLoading = false;
  visible = false;
  isEditMode = false;
  currentId: number | null = null;
  posForm!: FormGroup;

  private posApi = inject(PositionApiClient);
  private fb = inject(FormBuilder);

  ngOnInit() {
    this.posForm = this.fb.group({
      positionName: ['', [Validators.required, Validators.minLength(2)]]
    });
    this.loadPositions();
  }

  loadPositions() {
    this.isLoading = true;
    this.posApi.getPositions().subscribe({
      next: (data) => {
        this.positions = data;
        this.isLoading = false;
      },
      error: () => this.isLoading = false
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

    if (this.isEditMode && this.currentId) {
      this.posApi.updatePosition(this.currentId, name).subscribe({
        next: () => this.handleSuccess(),
        error: () => alert('Cập nhật thất bại!')
      });
    } else {
      const dto = new PositionDto();
      dto.positionName = name;
      this.posApi.createPosition(dto).subscribe({
        next: () => this.handleSuccess(),
        error: () => alert('Tạo mới thất bại!')
      });
    }
  }

  deletePos(id: number) {
    if (confirm('Xóa chức vụ này? (Chỉ xóa được nếu không có nhân viên nào giữ chức vụ này)')) {
      this.posApi.deletePosition(id).subscribe({
        next: () => this.loadPositions(),
        error: () => alert('Không thể xóa chức vụ đang có nhân sự!')
      });
    }
  }

  handleSuccess() {
    this.visible = false;
    this.loadPositions();
  }
}