import { Component, inject, DestroyRef, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-otp-verify',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './otp-verify.component.html',
  styleUrls: ['../login/login.component.scss']
})
export class OtpVerifyComponent implements OnInit {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private destroyRef = inject(DestroyRef);

  email = '';
  
  otpForm: FormGroup = this.fb.group({
    otp: ['', [Validators.required, Validators.pattern('^[0-9]{6}$')]]
  });

  isLoading = false;
  errorMessage = '';

  ngOnInit(): void {
    this.route.queryParams.pipe(takeUntilDestroyed(this.destroyRef)).subscribe(params => {
      this.email = params['email'] || '';
    });
  }

  onSubmit(): void {
    if (this.otpForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.verifyOtp(this.email, this.otpForm.value.otp)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          this.isLoading = false;
          // Chuyển tới trang reset password với token (mock)
          this.router.navigate(['/auth/reset-password'], { queryParams: { token: 'valid-token', email: this.email } });
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Mã OTP không hợp lệ hoặc đã hết hạn.';
        }
      });
  }
}
