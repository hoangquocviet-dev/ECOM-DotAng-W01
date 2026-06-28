import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminVoucherService } from '../../../../core/services/admin-voucher.service';
import { IVoucher } from '../../../../models/voucher.model';

@Component({
  selector: 'app-vouchers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './vouchers.component.html',
  styleUrls: ['./vouchers.component.scss']
})
export class VouchersComponent implements OnInit, OnDestroy {
  vouchers: IVoucher[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  constructor(private voucherService: AdminVoucherService) {}

  ngOnInit(): void {
    this.loadVouchers();
  }

  loadVouchers(): void {
    this.isLoading = true;
    this.voucherService.getVouchers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.vouchers = data;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }
  
  toggleStatus(voucher: IVoucher): void {
    this.voucherService.toggleStatus(voucher.id, !voucher.isActive)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          voucher.isActive = !voucher.isActive;
        },
        error: () => alert('Lỗi khi cập nhật trạng thái')
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
