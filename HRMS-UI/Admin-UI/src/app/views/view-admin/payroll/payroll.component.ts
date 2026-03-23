import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms'; // Thêm bộ này
import {
  SalaryApiClient,
  SalaryDto,
  CreateSalaryDto,
  EmployeeApiClient,
  EmployeeDto,
} from '../../../api/admin-api.service.generated';

@Component({
  selector: 'app-payroll',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule], // Nhớ thêm ReactiveFormsModule
  templateUrl: './payroll.component.html',
})
export class PayrollComponent implements OnInit {
  salaries: SalaryDto[] = [];
  employees: EmployeeDto[] = []; // Danh sách NV để chọn khi tạo lương
  salaryForm: FormGroup;
  showModal = false;
  isLoading = false;

  constructor(
    private salaryApi: SalaryApiClient,
    private employeeApi: EmployeeApiClient,
    private fb: FormBuilder,
    private cdr: ChangeDetectorRef,
  ) {
    // Khởi tạo Form khớp với CreateSalaryDto ở Backend
    this.salaryForm = this.fb.group({
      employeeCode: ['', Validators.required],
      luongCoBan: [0, [Validators.required, Validators.min(0)]],
      phuCap: [0],
      thuong: [0],
      haoHut: [0],
    });
  }

  ngOnInit(): void {
    this.loadData();
    this.loadEmployees();
  }

  loadData() {
    this.isLoading = true;
    this.salaryApi.getSalaries().subscribe({
      next: (res) => {
        this.salaries = res;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  loadEmployees() {
    this.employeeApi.getEmployees().subscribe((res) => (this.employees = res));
  }

  openModal() {
    this.salaryForm.reset({ luongCoBan: 0, phuCap: 0, thuong: 0, haoHut: 0 });
    this.showModal = true;
  }

  closeModal() {
    this.showModal = false;
  }

  submitSalary() {
    if (this.salaryForm.valid) {
      const dto = this.salaryForm.value as CreateSalaryDto;
      this.salaryApi.createSalary(dto).subscribe({
        next: () => {
          alert('Tạo bảng lương thành công!');
          this.closeModal();
          this.loadData();
        },
        error: (err) => alert('Lỗi: ' + err),
      });
    }
  }

  deleteSalary(id: number | undefined) {
    if (id && confirm('Xóa bản ghi này?')) {
      this.salaryApi.deleteSalary(id).subscribe(() => this.loadData());
    }
  }
}
