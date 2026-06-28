import { Component, OnInit, inject, DestroyRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HomeService } from '../../core/services/home.service';
import { IBanner } from '../../models/banner.model';
import { IProduct } from '../../models/product.model';
import { IFlashSale } from '../../models/flash-sale.model';
import { interval } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  private homeService = inject(HomeService);
  private destroyRef = inject(DestroyRef);

  banners: IBanner[] = [];
  featuredProducts: IProduct[] = [];
  newProducts: IProduct[] = []; // Explicitly handle new products
  flashSale: IFlashSale | null = null;

  activeBannerIndex = 0;
  
  // Flash sale countdown
  countdownHours = '00';
  countdownMinutes = '00';
  countdownSeconds = '00';
  private timerActive = false;

  ngOnInit(): void {
    this.loadBanners();
    this.loadFeaturedProducts();
    this.loadFlashSale();
  }

  loadBanners(): void {
    this.homeService.getBanners().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (data) => {
        this.banners = data;
        // Auto slide
        if (this.banners.length > 1) {
          interval(5000).pipe(takeUntilDestroyed(this.destroyRef)).subscribe(() => {
            this.nextBanner();
          });
        }
      }
    });
  }

  loadFeaturedProducts(): void {
    this.homeService.getFeaturedProducts().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (data) => {
        this.featuredProducts = data;
        // We simulate a 'New Products' section by taking products marked as new or the first 4 products
        this.newProducts = data.filter(p => p.isNew).slice(0, 4);
        if (this.newProducts.length === 0) {
          this.newProducts = data.slice(0, 4); // Fallback if no new products
        }
      }
    });
  }

  loadFlashSale(): void {
    this.homeService.getFlashSale().pipe(takeUntilDestroyed(this.destroyRef)).subscribe({
      next: (data) => {
        this.flashSale = data;
        if (this.flashSale) {
          this.startCountdown(new Date(this.flashSale.endTime));
        }
      }
    });
  }

  nextBanner(): void {
    if (this.banners.length > 0) {
      this.activeBannerIndex = (this.activeBannerIndex + 1) % this.banners.length;
    }
  }

  prevBanner(): void {
    if (this.banners.length > 0) {
      this.activeBannerIndex = this.activeBannerIndex === 0 ? this.banners.length - 1 : this.activeBannerIndex - 1;
    }
  }

  setBanner(index: number): void {
    this.activeBannerIndex = index;
  }

  startCountdown(endTime: Date): void {
    if (this.timerActive) return;
    this.timerActive = true;
    
    interval(1000).pipe(takeUntilDestroyed(this.destroyRef)).subscribe(() => {
      const now = new Date().getTime();
      const distance = endTime.getTime() - now;

      if (distance < 0) {
        this.countdownHours = '00';
        this.countdownMinutes = '00';
        this.countdownSeconds = '00';
        return;
      }

      const hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((distance % (1000 * 60)) / 1000);

      this.countdownHours = hours.toString().padStart(2, '0');
      this.countdownMinutes = minutes.toString().padStart(2, '0');
      this.countdownSeconds = seconds.toString().padStart(2, '0');
    });
  }

  trackById(index: number, item: any): number {
    return item.id || index;
  }
}
