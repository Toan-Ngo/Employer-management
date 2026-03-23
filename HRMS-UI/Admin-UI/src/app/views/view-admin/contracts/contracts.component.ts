import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  ContractApiClient,
  ContractDto,
  EmployeeApiClient,
  EmployeeDto,
  CreateContractDto,
  UpdateContractDto,
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
  AlertModule,
} from '@coreui/angular';

@Component({
  standalone: true,
  selector: 'app-contracts',
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
    AlertModule,
  ],
  templateUrl: './contracts.component.html',
})
export class ContractsComponent implements OnInit {
  contracts: ContractDto[] = [];
  employees: EmployeeDto[] = [];
  isLoading = false;
  visible = false;
  isEditMode = false;
  currentId: number | null = null;
  contractForm!: FormGroup;

  private contractApi = inject(ContractApiClient);
  private employeeApi = inject(EmployeeApiClient);
  private fb = inject(FormBuilder);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.initForm();
    this.loadContracts();
    this.loadEmployees();
  }

  initForm() {
    this.contractForm = this.fb.group({
      employeeCode: ['', Validators.required],
      contractName: ['', Validators.required],
      contractType: ['Hợp đồng lao động', Validators.required],
      salary: [0, [Validators.required, Validators.min(0)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      status: ['Hiệu lực'],
    });
  }

  checkStatus(endDate: any): string {
    if (!endDate) return 'Hiệu lực';
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    const end = new Date(endDate);
    return end < today ? 'Hết hiệu lực' : 'Hiệu lực';
  }

  loadContracts() {
    this.isLoading = true;
    this.cdr.detectChanges();
    this.contractApi.getContracts().subscribe({
      next: (data) => {
        // Sửa lỗi gạch chân đỏ bằng cách ép kiểu (as any[] hoặc as ContractDto[])
        this.contracts = data.map((item) => ({
          ...item,
          status: this.checkStatus(item.endDate),
        })) as ContractDto[];

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
    this.employeeApi.getEmployees().subscribe((data) => {
      this.employees = data.filter((emp) => emp.isActive === true);

      this.cdr.detectChanges();
    });
  }

  openEditModal(item: ContractDto) {
    this.isEditMode = true;
    this.currentId = item.id || null;

    this.contractForm.enable();
    this.contractForm.patchValue({
      employeeCode: item.employeeCode,
      contractName: item.contractName,
      contractType: item.contractType,
      salary: item.salary,
      startDate: this.formatDateForInput(item.startDate),
      endDate: this.formatDateForInput(item.endDate),
      status: this.checkStatus(item.endDate),
    });

    // Mờ các trường không cho phép sửa
    this.contractForm.get('contractName')?.disable();
    this.contractForm.get('employeeCode')?.disable();
    this.contractForm.get('contractType')?.disable();
    this.contractForm.get('salary')?.disable();
    this.contractForm.get('startDate')?.disable();
    this.contractForm.get('status')?.disable();

    this.visible = true;
    this.cdr.detectChanges();
  }

  openAddModal() {
    this.isEditMode = false;
    this.currentId = null;
    this.contractForm.enable();
    this.contractForm.reset({
      contractType: 'Hợp đồng lao động',
      status: 'Hiệu lực',
      salary: 0,
      contractName: '',
    });
    this.visible = true;
    this.cdr.detectChanges();
  }

  onSubmit() {
    if (this.contractForm.invalid) {
      this.contractForm.markAllAsTouched();
      return;
    }

    const val = this.contractForm.getRawValue();
    this.isLoading = true;
    this.cdr.detectChanges();

    const calculatedStatus = this.checkStatus(val.endDate);

    if (this.isEditMode && this.currentId) {
      const updateDto = new UpdateContractDto();
      updateDto.endDate = new Date(val.endDate);

      this.contractApi.updateContract(this.currentId, updateDto).subscribe({
        next: () => {
          alert(`Cập nhật thành công! Trạng thái: ${calculatedStatus}`);
          this.handleSuccess();
        },
        error: (err) => this.handleError(err),
      });
    } else {
      const createDto = new CreateContractDto();
      createDto.employeeCode = val.employeeCode;
      createDto.contractName = val.contractName;
      createDto.contractType = val.contractType;
      createDto.salary = Number(val.salary);
      createDto.startDate = new Date(val.startDate);
      createDto.endDate = new Date(val.endDate);
      (createDto as any).status = calculatedStatus;

      this.contractApi.createContract(createDto).subscribe({
        next: () => {
          alert(`Thêm mới thành công! Trạng thái: ${calculatedStatus}`);
          this.handleSuccess();
        },
        error: (err) => this.handleError(err),
      });
    }
  }

  private handleSuccess() {
    this.visible = false;
    this.isLoading = false;
    this.loadContracts();
    this.cdr.detectChanges();
  }

  private handleError(err: any) {
    this.isLoading = false;
    console.error(err);
    alert('Thao tác thất bại! Vui lòng kiểm tra lại dữ liệu.');
    this.cdr.detectChanges();
  }

  private formatDateForInput(date: any) {
    if (!date) return '';
    const d = new Date(date);
    return d.toISOString().split('T')[0];
  }
}
