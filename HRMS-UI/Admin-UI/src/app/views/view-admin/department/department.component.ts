import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  DepartmentApiClient,
  DepartmentDto,
} from '../../../api/admin-api.service.generated';
import {
  CardModule,
  GridModule,
  ButtonModule,
  TableModule,
  BadgeModule,
  ModalModule,
  FormModule,
  SpinnerModule,
} from '@coreui/angular';

@Component({
  standalone: true,
  selector: 'app-departments',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    GridModule,
    ButtonModule,
    TableModule,
    BadgeModule,
    ModalModule,
    FormModule,
    SpinnerModule,
  ],
  templateUrl: './department.component.html',
})
export class DepartmentComponent implements OnInit {
  departments: DepartmentDto[] = [];
  isLoading = false;
  visible = false;
  isEditMode = false;
  currentId: number | null = null;
  deptForm!: FormGroup;

  private cdr = inject(ChangeDetectorRef);
  private deptApi = inject(DepartmentApiClient);
  private fb = inject(FormBuilder);

  ngOnInit() {
    this.initForm();
    this.loadDepartments();
  }

  initForm() {
    this.deptForm = this.fb.group({
      departmentName: ['', [Validators.required, Validators.minLength(2)]],
    });
  }

  loadDepartments() {
    this.isLoading = true;
    this.cdr.detectChanges();

    this.deptApi.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Lỗi tải phòng ban:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  openAddModal() {
    this.isEditMode = false;
    this.currentId = null;
    this.deptForm.reset();
    this.visible = true;
  }

  openEditModal(dept: DepartmentDto) {
    this.isEditMode = true;
    this.currentId = dept.id || null;
    this.deptForm.patchValue({
      departmentName: dept.departmentName,
    });
    this.visible = true;
  }

  onSubmit() {
    if (this.deptForm.invalid) return;

    const name = this.deptForm.value.departmentName;
    this.isLoading = true;
    this.cdr.detectChanges();

    if (this.isEditMode && this.currentId) {
      const updateDto = new DepartmentDto();
      updateDto.id = this.currentId;
      updateDto.departmentName = name;

      this.deptApi.updateDepartment(this.currentId, updateDto).subscribe({
        next: () => this.handleSuccess('Cập nhật thành công'),
        error: (err) => this.handleError(err),
      });
    } else {
      const newDept = new DepartmentDto();
      newDept.departmentName = name;

      this.deptApi.createDepartment(newDept).subscribe({
        next: () => this.handleSuccess('Thêm mới thành công'),
        error: (err) => this.handleError(err),
      });
    }
  }

  deleteDept(id: number) {
    if (
      confirm(
        'Bạn có chắc chắn muốn xóa phòng ban này? (Lưu ý: Chỉ xóa được phòng trống)',
      )
    ) {
      this.deptApi.deleteDepartment(id).subscribe({
        next: () => this.loadDepartments(),
        error: (err) =>
          alert('Không thể xóa! Phòng đang có nhân viên hoặc lỗi hệ thống.'),
      });
    }
  }

  private handleSuccess(msg: string) {
    this.isLoading = false;
    this.visible = false;
    this.loadDepartments();
    console.log(msg);
  }

  private handleError(err: any) {
    this.isLoading = false;
    this.cdr.detectChanges();
    console.error('Lỗi API:', err);
    alert('Thao tác thất bại. Vui lòng kiểm tra lại dữ liệu.');
  }
}
