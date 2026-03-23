import {
  AuthApiClient,
  AuthenticatedResult,
  LoginRequest,
  ForgotPasswordDto,
  ResetPasswordDto,
} from './../../../api/admin-api.service.generated';
import { Component, OnDestroy } from '@angular/core';
import { IconDirective } from '@coreui/icons-angular';
import { UrlConstants } from '../../../shared/constants/url.constants';
import {
  ButtonDirective,
  CardBodyComponent,
  CardComponent,
  CardGroupComponent,
  ColComponent,
  ContainerComponent,
  FormControlDirective,
  FormDirective,
  InputGroupComponent,
  InputGroupTextDirective,
  RowComponent,
  SpinnerComponent,
} from '@coreui/angular';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { AlertService } from '../../../shared/service/alert.service';
import { Router } from '@angular/router';
import { TokenStorageService } from '../../../shared/service/token-storage.service';
import { Subject, takeUntil } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ContainerComponent,
    RowComponent,
    ColComponent,
    CardGroupComponent,
    CardComponent,
    CardBodyComponent,
    FormDirective,
    InputGroupComponent,
    InputGroupTextDirective,
    IconDirective,
    FormControlDirective,
    ButtonDirective,
    FormsModule,
    ReactiveFormsModule,
    SpinnerComponent,
  ],
})
export class LoginComponent implements OnDestroy {
  loginForm: FormGroup;
  forgotForm: FormGroup;
  resetForm: FormGroup;

  mode: 'login' | 'forgot' | 'reset' = 'login';
  loading = false;
  private ngUnsubscribe = new Subject<void>();

  constructor(
    private fb: FormBuilder,
    private authApiClient: AuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private tokenService: TokenStorageService,
  ) {
    this.loginForm = this.fb.group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });

    this.forgotForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });

    this.resetForm = this.fb.group({
      email: ['', Validators.required],
      code: [
        '',
        [Validators.required, Validators.minLength(6), Validators.maxLength(6)],
      ],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }

  switchMode(newMode: 'login' | 'forgot' | 'reset') {
    this.mode = newMode;
  }

  login() {
    if (this.loginForm.invalid || this.loading) return;
    this.loading = true;

    const request: LoginRequest = new LoginRequest({
      userName: this.loginForm.controls['userName'].value,
      password: this.loginForm.controls['password'].value,
    });

    this.authApiClient
      .login(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: AuthenticatedResult) => {
          this.loading = false;
          const user = this.tokenService.decodeUserFromToken(res.token);

          if (user) {
            this.tokenService.saveUser(user);
            this.tokenService.saveToken(res.token);

            const permissions: string[] = user.permissions;

            if (
              permissions.includes('Permissions.Account.View') ||
              permissions.includes('Permissions.Employee.View')
            ) {
              this.router.navigate([UrlConstants.HOMEADMIN]);
            } else if (permissions.includes('Permissions.Attendance.View')) {
              this.router.navigate([UrlConstants.HOMEEMPLOYEE]);
            } else {
              this.router.navigate(['/403']);
            }
          }
        },
        error: (err: any) => {
          console.log(err);
          this.alertService.showError(
            'Tên đăng nhập hoặc mật khẩu không chính xác.',
          );
          this.loading = false;
        },
      });
  }

  requestReset() {
    if (this.forgotForm.invalid || this.loading) return;
    this.loading = true;

    const email = this.forgotForm.value.email;
    const request = new ForgotPasswordDto({ email: email });

    this.authApiClient
      .forgotPassword(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.loading = false;
          this.alertService.showSuccess(
            'Nếu email tồn tại, mã xác nhận đã được gửi!',
          );
          this.resetForm.patchValue({ email: email });
          this.switchMode('reset');
        },
        error: (err) => {
          this.loading = false;
          this.alertService.showError('Có lỗi xảy ra, vui lòng thử lại sau.');
          console.error(err);
        },
      });
  }

  confirmReset() {
    if (this.resetForm.invalid || this.loading) return;
    this.loading = true;

    const request = new ResetPasswordDto({
      email: this.resetForm.value.email,
      code: this.resetForm.value.code,
      newPassword: this.resetForm.value.newPassword,
    });

    this.authApiClient
      .resetPassword(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: () => {
          this.loading = false;
          this.alertService.showSuccess(
            'Đổi mật khẩu thành công! Vui lòng đăng nhập lại.',
          );
          this.switchMode('login');
          this.loginForm.reset();
          this.forgotForm.reset();
          this.resetForm.reset();
        },
        error: (err) => {
          this.loading = false;
          this.alertService.showError(
            'Mã xác nhận không đúng hoặc đã hết hạn.',
          );
          console.error(err);
        },
      });
  }
}
