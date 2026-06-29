import { Component, inject, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'] // Reusing login styles or separate if needed
})
export class RegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef);

  registerForm: FormGroup = this.fb.group({
    name: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    username: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  isLoading = false;
  errorMessage = '';

  onSubmit(): void {
    if (this.registerForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    this.authService.register(this.registerForm.value)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (res) => {
          this.isLoading = false;
          // Chuyển tới trang đăng nhập hoặc xác thực OTP
          this.router.navigate(['/auth/login']); 
        },
        error: (err) => {
          this.isLoading = false;
          this.errorMessage = 'Đăng ký thất bại. Vui lòng kiểm tra lại.';
          console.error(err);
        }
      });
  }

  loginWithGoogle(): void {
    window.location.href = `${environment.apiUrl}/Users/auth/google`;
  }

  loginWithFacebook(): void {
    window.location.href = `${environment.apiUrl}/Users/auth/facebook`;
  }
}
