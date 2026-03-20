import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators, FormsModule } from '@angular/forms';
import {
  CardBodyComponent, RowComponent, ColComponent, CardHeaderComponent, CardComponent,
  FormControlDirective, FormSelectDirective, ButtonDirective, TableDirective,
  TableColorDirective, BadgeComponent,
  ModalComponent, ModalHeaderComponent, ModalTitleDirective,
  ButtonCloseDirective, ModalBodyComponent, ModalFooterComponent, FormLabelDirective
} from "@coreui/angular";

import {
  EmployeeApiClient,
  EmployeeDto,
  CreateEmployeeDto,
  UpdateEmployeeDto,
  DepartmentApiClient,
  DepartmentDto,
  PositionApiClient,
  PositionDto
} from '../../api/admin-api.service.generated';

@Component({
  standalone: true,
  selector: 'app-employees',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CardBodyComponent, RowComponent, ColComponent, CardHeaderComponent, CardComponent,
    FormControlDirective, FormSelectDirective, ButtonDirective, TableDirective,
    TableColorDirective, BadgeComponent, FormLabelDirective,
    ModalComponent, ModalHeaderComponent, ModalTitleDirective,
    ButtonCloseDirective, ModalBodyComponent, ModalFooterComponent
  ],
  templateUrl: './employees.component.html',
  styleUrl: './employees.component.scss',
})
export class EmployeesComponent implements OnInit {
  employees: EmployeeDto[] = [];
  allEmployees: EmployeeDto[] = [];
  departments: DepartmentDto[] = []; 
  positions: PositionDto[] = [];

  isLoading = false;
  searchTerm = '';
  selectedDepartment = '';

  visible = false;
  isEditMode = false;
  currentEmployeeCode = '';
  employeeForm!: FormGroup;

  private employeeApi = inject(EmployeeApiClient);
  private departmentApi = inject(DepartmentApiClient);
  private positionApi = inject(PositionApiClient);
  private fb = inject(FormBuilder);

  ngOnInit() {
    this.initForm();
    this.loadEmployees();
    this.loadDepartments();
    this.loadPositions();
  }

  initForm() {
    this.employeeForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      gender: [0, Validators.required],
      address: [''],
      departmentName: ['', Validators.required],
      positionName: ['', Validators.required] 
    });
  }

  loadPositions() {
    this.positionApi.getPositions().subscribe({
      next: (data) => this.positions = data,
      error: (err) => console.error('Lỗi tải chức vụ:', err)
    });
  }

  loadDepartments() {
    this.departmentApi.getDepartments().subscribe({
      next: (data) => this.departments = data,
      error: (err) => console.error('Lỗi tải phòng ban:', err)
    });
  }

  loadEmployees() {
    this.isLoading = true;
    this.employeeApi.getEmployees().subscribe({
      next: (data: EmployeeDto[]) => {
        this.allEmployees = data;
        this.employees = [...this.allEmployees];
        this.isLoading = false;
      },
      error: (err) => {
        console.error('Lỗi khi tải danh sách:', err);
        this.isLoading = false;
      }
    });
  }

  search() {
    const term = this.searchTerm.toLowerCase().trim();
    const dept = this.selectedDepartment;

    this.employees = this.allEmployees.filter(emp => {
      const matchTerm = !term ||
        (emp.employeeCode ?? '').toLowerCase().includes(term) ||
        (emp.fullName ?? '').toLowerCase().includes(term);

      const matchDept = !dept || emp.departmentName === dept;
      return matchTerm && matchDept;
    });
  }

  openAddModal() {
    this.isEditMode = false;
    this.employeeForm.reset({ gender: 0, departmentName: '', positionName: '' });
    this.visible = true;
  }

  openEditModal(emp: EmployeeDto) {
    this.isEditMode = true;
    this.currentEmployeeCode = emp.employeeCode ?? ''; 

    const nameParts = (emp.fullName ?? '').split(' ');
    this.employeeForm.patchValue({
      firstName: nameParts[0] || '',
      lastName: nameParts.slice(1).join(' ') || '',
      email: 'hidden@example.com', // Cập nhật nếu DTO có Email
      phone: '0000000000',
      dateOfBirth: '2000-01-01',
      gender: 0,
      departmentName: emp.departmentName,
      positionName: emp.positionName
    });
    this.visible = true;
  }

  handleModalChange(event: any) {
    this.visible = event;
  }

  onSubmit() {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const formValue = this.employeeForm.value;
    this.isLoading = true;

    if (this.isEditMode) {
      const updateDto = new UpdateEmployeeDto();
      updateDto.employeeCode = this.currentEmployeeCode;
      updateDto.departmentName = formValue.departmentName;
      updateDto.positionName = formValue.positionName;

      this.employeeApi.updateEmployee(updateDto).subscribe({
        next: () => this.finishSubmit(),
        error: () => {
          this.isLoading = false;
          alert('Cập nhật thất bại.');
        }
      });
    } else {
      const createDto = new CreateEmployeeDto();
      createDto.firstName = formValue.firstName;
      createDto.lastName = formValue.lastName;
      createDto.email = formValue.email;
      createDto.phone = formValue.phone;
      createDto.address = formValue.address;
      createDto.gender = Number(formValue.gender);
      createDto.departmentName = formValue.departmentName;
      createDto.positionName = formValue.positionName;

      if (formValue.dateOfBirth) {
        createDto.dateOfBirth = new Date(formValue.dateOfBirth);
      }

      this.employeeApi.createEmployee(createDto).subscribe({
        next: () => {
          alert('Tạo nhân viên thành công!');
          this.finishSubmit();
        },
        error: (err) => {
          this.isLoading = false;
          alert('Lỗi tạo mới. Vui lòng kiểm tra lại phòng ban/chức vụ.');
        }
      });
    }
  }

  private finishSubmit() {
    this.visible = false;
    this.loadEmployees();
    this.isLoading = false;
  }

  deleteEmployee(emp: EmployeeDto) {
    if (confirm(`Bạn có chắc chắn muốn xác nhận nhân viên ${emp.fullName} nghỉ việc?`)) {
      this.isLoading = true;
      this.employeeApi.deleteEmployee(emp.employeeCode ?? '').subscribe({
        next: () => this.loadEmployees(),
        error: (err) => {
          console.error('Lỗi khi xử lý:', err);
          this.isLoading = false;
        }
      });
    }
  }
}