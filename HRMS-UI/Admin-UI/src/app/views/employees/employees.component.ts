import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
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

  private cdr = inject(ChangeDetectorRef);
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
    this.isLoading = true;
    this.cdr.detectChanges();

    this.departmentApi.getDepartments().subscribe({
      next: (data) => {
        this.departments = data;
        this.isLoading = false;


        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Lỗi tải phòng ban:', err);
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
  loadEmployees() {
    this.isLoading = true;
    this.employeeApi.getEmployees().subscribe({
      next: (data) => {
        this.allEmployees = data;
        this.employees = [...this.allEmployees];
        this.isLoading = false;
        this.employees = data.filter(emp => emp.isActive === true);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.cdr.detectChanges();
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
      // Thay vì khởi tạo class rỗng, hãy tạo object literal để đảm bảo thuộc tính tồn tại
      const updateDto: UpdateEmployeeDto = {
        employeeCode: this.currentEmployeeCode, // Đây là mã lấy từ dòng bạn đã chọn
        departmentName: formValue.departmentName,
        positionName: formValue.positionName
      } as UpdateEmployeeDto;

      // Log ra để kiểm tra chắc chắn trước khi gọi API
      console.log('Payload gửi đi:', updateDto);

      this.employeeApi.updateEmployee(updateDto).subscribe({
        next: () => {
          alert('Cập nhật thành công!');
          this.finishSubmit();
        },
        error: (err) => {
          this.isLoading = false;
          // In chi tiết lỗi từ Server trả về để debug nếu vẫn lỗi 400
          console.error('Lỗi chi tiết từ Server:', err);
          alert('Cập nhật thất bại. Vui lòng kiểm tra lại mã nhân viên.');
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