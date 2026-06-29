import { Routes } from '@angular/router';

export const AUTH_ROUTES: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./login/login.component').then(m => m.LoginComponent)
  },
  {
    path: 'register',
    loadComponent: () => import('./register/register.component').then(m => m.RegisterComponent)
  },
  {
    path: 'forgot-password',
    loadComponent: () => import('./forgot-password/forgot-password.component').then(m => m.ForgotPasswordComponent)
  },
  {
    path: 'reset-password',
    loadComponent: () => import('./reset-password/reset-password.component').then(m => m.ResetPasswordComponent)
  },
  {
    path: 'verify-otp',
    loadComponent: () => import('./otp-verify/otp-verify.component').then(m => m.OtpVerifyComponent)
  },
  {
    path: 'oauth-callback',
    loadComponent: () => import('./oauth-callback/oauth-callback.component').then(m => m.OauthCallbackComponent)
  },
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  }
];
