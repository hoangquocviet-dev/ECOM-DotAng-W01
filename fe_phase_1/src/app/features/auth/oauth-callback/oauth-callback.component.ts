import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-oauth-callback',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './oauth-callback.component.html',
  styleUrls: ['./oauth-callback.component.scss']
})
export class OauthCallbackComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  ngOnInit(): void {
    // Read the query parameters from URL
    // e.g., /auth/oauth-callback?token=eyJhbGci...
    this.route.queryParams.subscribe(params => {
      const token = params['token'];
      if (token) {
        // Store JWT token
        localStorage.setItem('token', token);
        // Redirect to home page or intended destination
        this.router.navigate(['/']);
      } else {
        // If no token is found, redirect to login page with error
        console.error('OAuth login failed: No token received.');
        this.router.navigate(['/auth/login']);
      }
    });
  }
}
