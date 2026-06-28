import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminFlashSaleService } from '../../../../core/services/admin-flash-sale.service';
import { IFlashSale } from '../../../../models/flash-sale.model';

@Component({
  selector: 'app-flash-sales',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './flash-sales.component.html',
  styleUrls: ['./flash-sales.component.scss']
})
export class FlashSalesComponent implements OnInit, OnDestroy {
  flashSales: IFlashSale[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  constructor(private flashSaleService: AdminFlashSaleService) {}

  ngOnInit(): void {
    this.loadFlashSales();
  }

  loadFlashSales(): void {
    this.isLoading = true;
    this.flashSaleService.getFlashSales()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.flashSales = data;
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
