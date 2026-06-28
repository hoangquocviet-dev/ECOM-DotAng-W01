import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminBannerService } from '../../../../core/services/admin-banner.service';
import { IBanner } from '../../../../models/banner.model';

@Component({
  selector: 'app-banners',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './banners.component.html',
  styleUrls: ['./banners.component.scss']
})
export class BannersComponent implements OnInit, OnDestroy {
  banners: IBanner[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  constructor(private bannerService: AdminBannerService) {}

  ngOnInit(): void {
    this.loadBanners();
  }

  loadBanners(): void {
    this.isLoading = true;
    this.bannerService.getBanners()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.banners = data.sort((a, b) => a.displayOrder - b.displayOrder);
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
