import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { HomeService } from '../../core/services/home.service';
import { IBanner } from '../../models/banner.model';
import { IProduct } from '../../models/product.model';
import { IFlashSale } from '../../models/flash-sale.model';
import { Subscription, interval } from 'rxjs';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, OnDestroy {
  banners: IBanner[] = [];
  featuredProducts: IProduct[] = [];
  flashSale: IFlashSale | null = null;

  activeBannerIndex = 0;
  
  // Flash sale countdown
  countdownHours = '00';
  countdownMinutes = '00';
  countdownSeconds = '00';
  private timerSubscription?: Subscription;

  constructor(private homeService: HomeService) {}

  ngOnInit(): void {
    this.loadBanners();
    this.loadFeaturedProducts();
    this.loadFlashSale();
  }

  ngOnDestroy(): void {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
    }
  }

  loadBanners(): void {
    this.homeService.getBanners().subscribe({
      next: (data) => {
        this.banners = data;
        // Auto slide
        if (this.banners.length > 1) {
          setInterval(() => {
            this.nextBanner();
          }, 5000);
        }
      }
    });
  }

  loadFeaturedProducts(): void {
    this.homeService.getFeaturedProducts().subscribe({
      next: (data) => this.featuredProducts = data
    });
  }

  loadFlashSale(): void {
    this.homeService.getFlashSale().subscribe({
      next: (data) => {
        this.flashSale = data;
        if (this.flashSale) {
          this.startCountdown(new Date(this.flashSale.endTime));
        }
      }
    });
  }

  nextBanner(): void {
    this.activeBannerIndex = (this.activeBannerIndex + 1) % this.banners.length;
  }

  prevBanner(): void {
    this.activeBannerIndex = this.activeBannerIndex === 0 ? this.banners.length - 1 : this.activeBannerIndex - 1;
  }

  setBanner(index: number): void {
    this.activeBannerIndex = index;
  }

  startCountdown(endTime: Date): void {
    this.timerSubscription = interval(1000).subscribe(() => {
      const now = new Date().getTime();
      const distance = endTime.getTime() - now;

      if (distance < 0) {
        if (this.timerSubscription) {
          this.timerSubscription.unsubscribe();
        }
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
    return item.id;
  }
}
