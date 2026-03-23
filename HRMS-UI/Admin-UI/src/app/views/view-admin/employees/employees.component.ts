import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
} from '@angular/forms';
import {
  CardBodyComponent,
  RowComponent,
  ColComponent,
  CardHeaderComponent,
  CardComponent,
  FormControlDirective,
  FormSelectDirective,
  ButtonDirective,
  TableDirective,
  TableColorDirective,
  BadgeComponent,
  ModalComponent,
  ModalHeaderComponent,
  ModalTitleDirective,
  ButtonCloseDirective,
  ModalBodyComponent,
  ModalFooterComponent,
  FormLabelDirective,
} from '@coreui/angular';

import {
  EmployeeApiClient,
  EmployeeDto,
  CreateEmployeeDto,
  UpdateEmployeeDto,
  DepartmentApiClient,
  DepartmentDto,
  PositionApiClient,
  PositionDto,
  AccountApiClient,
  CreateAccountDto,
} from '../../../api/admin-api.service.generated';

// Sửa lại đường dẫn import TokenStorageService cho đúng với project của bạn
import { TokenStorageService } from '../../../shared/service/token-storage.service';

@Component({
  standalone: true,
  selector: 'app-employees',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CardBodyComponent,
    RowComponent,
    ColComponent,
    CardHeaderComponent,
    CardComponent,
    FormControlDirective,
    FormSelectDirective,
    ButtonDirective,
    TableDirective,
    TableColorDirective,
    BadgeComponent,
    FormLabelDirective,
    ModalComponent,
    ModalHeaderComponent,
    ModalTitleDirective,
    ButtonCloseDirective,
    ModalBodyComponent,
    ModalFooterComponent,
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
  userPermissions: string[] = []; // Biến lưu trữ quyền để ẩn/hiện nút trên HTML

  private cdr = inject(ChangeDetectorRef);
  private employeeApi = inject(EmployeeApiClient);
  private departmentApi = inject(DepartmentApiClient);
  private positionApi = inject(PositionApiClient);
  private accountApi = inject(AccountApiClient);
  private fb = inject(FormBuilder);
  private tokenService = inject(TokenStorageService);

  ngOnInit() {
    this.initForm();
    this.loadEmployees();
    this.loadDepartments();
    this.loadPositions();

    // Lấy danh sách quyền của người đang đăng nhập
    const user = this.tokenService.getUser();
    if (user && user.permissions) {
      this.userPermissions = user.permissions;
    }
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
      positionName: ['', Validators.required],
      role: ['Employee', Validators.required],
    });
  }

  loadPositions() {
    this.positionApi.getPositions().subscribe({
      next: (data) => (this.positions = data),
      error: (err) => console.error('Lỗi tải chức vụ:', err),
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
      },
    });
  }

  loadEmployees() {
    this.isLoading = true;
    this.employeeApi.getEmployees().subscribe({
      next: (data) => {
        this.allEmployees = data;
        this.employees = [...this.allEmployees];
        this.isLoading = false;
        this.employees = data.filter((emp) => emp.isActive === true);
        this.cdr.detectChanges();
      },
      error: (err) => {
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  search() {
    const term = this.searchTerm.toLowerCase().trim();
    const dept = this.selectedDepartment;

    this.employees = this.allEmployees.filter((emp) => {
      const matchTerm =
        !term ||
        (emp.employeeCode ?? '').toLowerCase().includes(term) ||
        (emp.fullName ?? '').toLowerCase().includes(term);

      const matchDept = !dept || emp.departmentName === dept;
      return matchTerm && matchDept;
    });
  }

  openAddModal() {
    this.isEditMode = false;
    this.employeeForm.reset({
      gender: 0,
      departmentName: '',
      positionName: '',
      role: 'Employee',
    });
    this.visible = true;
  }

  openEditModal(emp: EmployeeDto) {
    this.isEditMode = true;
    this.currentEmployeeCode = emp.employeeCode ?? '';

    const nameParts = (emp.fullName ?? '').split(' ');
    this.employeeForm.patchValue({
      firstName: nameParts[0] || '',
      lastName: nameParts.slice(1).join(' ') || '',
      email: 'hidden@example.com',
      phone: '0000000000',
      dateOfBirth: '2000-01-01',
      gender: 0,
      departmentName: emp.departmentName,
      positionName: emp.positionName,
      role: 'Employee',
    });
    this.visible = true;
  }

  handleModalChange(event: any) {
    this.visible = event;
  }

  private removeVietnameseTones(str: string): string {
    str = str.replace(/à|á|ạ|ả|ã|â|ầ|ấ|ậ|ẩ|ẫ|ă|ằ|ắ|ặ|ẳ|ẵ/g, 'a');
    str = str.replace(/è|é|ẹ|ẻ|ẽ|ê|ề|ế|ệ|ể|ễ/g, 'e');
    str = str.replace(/ì|í|ị|ỉ|ĩ/g, 'i');
    str = str.replace(/ò|ó|ọ|ỏ|õ|ô|ồ|ố|ộ|ổ|ỗ|ơ|ờ|ớ|ợ|ở|ỡ/g, 'o');
    str = str.replace(/ù|ú|ụ|ủ|ũ|ư|ừ|ứ|ự|ử|ữ/g, 'u');
    str = str.replace(/ỳ|ý|ỵ|ỷ|ỹ/g, 'y');
    str = str.replace(/đ/g, 'd');
    str = str.replace(/À|Á|Ạ|Ả|Ã|Â|Ầ|Ấ|Ậ|Ẩ|Ẫ|Ă|Ằ|Ắ|Ặ|Ẳ|Ẵ/g, 'A');
    str = str.replace(/È|É|Ẹ|Ẻ|Ẽ|Ê|Ề|Ế|Ệ|Ể|Ễ/g, 'E');
    str = str.replace(/Ì|Í|Ị|Ỉ|Ĩ/g, 'I');
    str = str.replace(/Ò|Ó|Ọ|Ỏ|Õ|Ô|Ồ|Ố|Ộ|Ổ|Ỗ|Ơ|Ờ|Ớ|Ợ|Ở|Ỡ/g, 'O');
    str = str.replace(/Ù|Ú|Ụ|Ủ|Ũ|Ư|Ừ|Ứ|Ự|Ử|Ữ/g, 'U');
    str = str.replace(/Ỳ|Ý|Ỵ|Ỷ|Ỹ/g, 'Y');
    str = str.replace(/Đ/g, 'D');
    str = str.replace(/\u0300|\u0301|\u0303|\u0309|\u0323/g, '');
    str = str.replace(/\u02C6|\u0306|\u031B/g, '');
    return str.replace(/\s+/g, '');
  }

  private downloadCredentials(
    fullName: string,
    email: string,
    username: string,
    pass: string,
    role: string,
  ) {
    const content = `THÔNG TIN TÀI KHOẢN NHÂN VIÊN MỚI
----------------------------------------
Họ và tên: ${fullName}
Email đăng ký: ${email}
Quyền hạn: ${role}

Tài khoản (Username): ${username}
Mật khẩu (Password): ${pass}

* Vui lòng gửi thông tin này cho nhân viên và nhắc họ đổi mật khẩu trong lần đăng nhập đầu tiên!`;

    const blob = new Blob([content], { type: 'text/plain;charset=utf-8' });
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = `TaiKhoan_${username}.txt`;
    link.click();
    window.URL.revokeObjectURL(url);
  }

  onSubmit() {
    if (this.employeeForm.invalid) {
      this.employeeForm.markAllAsTouched();
      return;
    }

    const formValue = this.employeeForm.value;
    this.isLoading = true;

    if (this.isEditMode) {
      const updateDto: UpdateEmployeeDto = {
        employeeCode: this.currentEmployeeCode,
        departmentName: formValue.departmentName,
        positionName: formValue.positionName,
      } as UpdateEmployeeDto;

      this.employeeApi.updateEmployee(updateDto).subscribe({
        next: () => {
          alert('Cập nhật thành công!');
          this.finishSubmit();
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Lỗi chi tiết từ Server:', err);
          alert('Cập nhật thất bại. Vui lòng kiểm tra lại mã nhân viên.');
        },
      });
    } else {
      this.accountApi.getAccounts().subscribe({
        next: (accounts) => {
          const emailExists = accounts.some(
            (a) =>
              a.email?.toLowerCase() === formValue.email.toLowerCase().trim(),
          );

          if (emailExists) {
            this.isLoading = false;
            alert(
              'CẢNH BÁO: Email này đã được sử dụng cho một tài khoản khác! Vui lòng nhập Email khác.',
            );
            this.cdr.detectChanges();
            return;
          }

          this.proceedCreateEmployee(formValue);
        },
        error: (err) => {
          this.isLoading = false;
          console.error('Lỗi kiểm tra tài khoản:', err);
          alert('Không thể kiểm tra dữ liệu tài khoản. Vui lòng thử lại sau.');
        },
      });
    }
  }

  private proceedCreateEmployee(formValue: any) {
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
        const accountDto = new CreateAccountDto();
        accountDto.email = formValue.email;

        const rawName = `${formValue.lastName}${formValue.firstName}`;
        const cleanName = this.removeVietnameseTones(rawName);
        const randomNum = Math.floor(100 + Math.random() * 900);
        accountDto.user = `${cleanName.toLowerCase()}${randomNum}`;

        let namePart = cleanName.substring(0, 3);
        if (namePart.length < 3) namePart = namePart.padEnd(3, 'x');
        namePart =
          namePart.charAt(0).toUpperCase() + namePart.slice(1).toLowerCase();

        const dobString = formValue.dateOfBirth
          ? formValue.dateOfBirth.replace(/-/g, '')
          : '20000101';
        const dobPart = dobString.slice(-4);

        accountDto.password = `${namePart}@${dobPart}`;
        (accountDto as any).role = formValue.role;

        this.accountApi.createAccount(accountDto).subscribe({
          next: () => {
            const fullName = `${formValue.lastName} ${formValue.firstName}`;
            this.downloadCredentials(
              fullName,
              accountDto.email!,
              accountDto.user!,
              accountDto.password!,
              formValue.role,
            );

            alert(
              `Tạo nhân viên thành công!\n\n` +
                `Hệ thống đã tự động tải xuống file thông tin đăng nhập.\n` +
                `👤 Username: ${accountDto.user}\n` +
                `🔑 Password: ${accountDto.password}\n` +
                `🛡️ Quyền: ${formValue.role}`,
            );
            this.finishSubmit();
          },
          error: (accErr) => {
            this.isLoading = false;
            console.error('Lỗi tạo tài khoản:', accErr);
            alert(
              'CẢNH BÁO: Đã tạo hồ sơ nhân viên, nhưng lỗi khi cấp tài khoản đăng nhập. Vui lòng báo kỹ thuật kiểm tra.',
            );
            this.finishSubmit();
          },
        });
      },
      error: (err) => {
        this.isLoading = false;
        alert('Lỗi tạo mới hồ sơ. Vui lòng kiểm tra lại phòng ban/chức vụ.');
      },
    });
  }

  private finishSubmit() {
    this.visible = false;
    this.loadEmployees();
    this.isLoading = false;
  }

  deleteEmployee(emp: EmployeeDto) {
    if (
      confirm(
        `Bạn có chắc chắn muốn xác nhận nhân viên ${emp.fullName} nghỉ việc?`,
      )
    ) {
      this.isLoading = true;
      this.employeeApi.deleteEmployee(emp.employeeCode ?? '').subscribe({
        next: () => this.loadEmployees(),
        error: (err) => {
          console.error('Lỗi khi xử lý:', err);
          this.isLoading = false;
        },
      });
    }
  }
}
