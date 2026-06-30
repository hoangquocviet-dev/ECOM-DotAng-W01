import { Component, inject, OnInit, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserService, UserProfile } from '../../services/user.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-info',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './info.component.html',
  styleUrl: './info.component.scss'
})
export class InfoComponent implements OnInit {
  private userService = inject(UserService);
  private destroyRef = inject(DestroyRef);
  
  userProfile: UserProfile | null = null;
  isLoading = true;

  ngOnInit(): void {
    this.userService.getProfile()
      .pipe(takeUntilDestroyed(this.destroyRef))
      .subscribe({
        next: (profile) => {
          this.userProfile = profile;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  copyReferralCode(): void {
    if (this.userProfile?.referralCode) {
      navigator.clipboard.writeText(this.userProfile.referralCode);
      alert('Đã copy mã giới thiệu!');
    }
  }
}
