import {
  AuthApiClient,
  AuthenticatedResult,
  LoginRequest,
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
import { TokenStorageService } from 'src/app/shared/service/token-storage.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  imports: [
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
  ],
})
export class LoginComponent implements OnDestroy {
  loginForm: FormGroup;
  private ngUnsubscribe = new Subject<void>();
  loading = false;
  constructor(
    private fb: FormBuilder,
    private authApiClient: AuthApiClient,
    private alertService: AlertService,
    private router: Router,
    private tokenService: TokenStorageService
  ) {
    this.loginForm = this.fb.group({
      userName: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }
  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
  login() {
    if (this.loading) return;
    this.loading = true;

    const request: LoginRequest = new LoginRequest({
      userName: this.loginForm.controls['userName'].value,
      password: this.loginForm.controls['password'].value,
    });

    this.authApiClient.login(request)
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe({
        next: (res: AuthenticatedResult) => {
          this.loading = false;

          // decode token -> UserModel
          const user = this.tokenService.decodeUserFromToken(res.token);

          if (user) {
            this.tokenService.saveUser(user);
            this.tokenService.saveToken(res.token);
            this.tokenService.saveRefreshToken(res.refreshToken);
          }

          this.router.navigate([UrlConstants.HOME]);
        },
        error: (err: any) => {
          console.log(err);
          this.alertService.showError('Login invalid');
          this.loading = false;
        },
      });
  }
}
