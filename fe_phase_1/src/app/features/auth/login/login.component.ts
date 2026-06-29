import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { Component, inject, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators, FormGroup } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private destroyRef = inject(DestroyRef); // Dùng để tránh memory leak

  loginForm: FormGroup = this.fb.group({
    emailOrUsername: ['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  isLoading = false;
  errorMessage = '';

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.isLoading = true;
    this.errorMessage = '';

    const payload = {
      username: this.loginForm.value.emailOrUsername,
      password: this.loginForm.value.password
    };

    this.authService.login(payload)
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
      next: (res) => {
        this.isLoading = false;
        this.router.navigate(['/']);
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = 'Đăng nhập thất bại. Vui lòng kiểm tra lại.';
      }
    });
  }

  loginWithGoogle(): void {
    // Redirect to backend OAuth2 Google endpoint
    window.location.href = `${environment.apiUrl}/Users/auth/google`;
  }

  loginWithFacebook(): void {
    // Redirect to backend OAuth2 Facebook endpoint
    window.location.href = `${environment.apiUrl}/Users/auth/facebook`;
  }
}
