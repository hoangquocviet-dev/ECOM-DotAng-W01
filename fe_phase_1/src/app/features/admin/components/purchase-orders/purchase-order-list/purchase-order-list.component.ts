import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminPurchaseOrderService } from '../../../../../core/services/admin-purchase-order.service';
import { IPurchaseOrder } from '../../../../../models/purchase-order.model';

@Component({
  selector: 'app-purchase-order-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './purchase-order-list.component.html',
  styleUrls: ['./purchase-order-list.component.scss']
})
export class PurchaseOrderListComponent implements OnInit, OnDestroy {
  orders: IPurchaseOrder[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  constructor(private poService: AdminPurchaseOrderService) {}

  ngOnInit(): void {
    this.loadOrders();
  }

  loadOrders(): void {
    this.isLoading = true;
    this.poService.getPurchaseOrders()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.orders = data;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading purchase orders', error);
          this.isLoading = false;
        }
      });
  }

  completeOrder(id: string): void {
    if (confirm('Hoàn tất phiếu nhập này sẽ cập nhật số lượng tồn kho của các sản phẩm. Bạn có chắc chắn?')) {
      this.poService.completePurchaseOrder(id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            const order = this.orders.find(o => o.id === id);
            if (order) {
              order.status = 'COMPLETED';
            }
            alert('Đã hoàn tất phiếu nhập kho!');
          },
          error: () => alert('Có lỗi xảy ra!')
        });
    }
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'COMPLETED': return 'badge-success';
      case 'DRAFT': return 'badge-warning';
      case 'CANCELLED': return 'badge-danger';
      default: return 'badge-secondary';
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
