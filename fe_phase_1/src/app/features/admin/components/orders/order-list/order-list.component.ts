import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdminOrderService, Order } from '../../../../../core/services/admin-order.service';

@Component({
  selector: 'app-order-list',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './order-list.component.html',
  styleUrls: ['./order-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderListComponent {
  private orderService = inject(AdminOrderService);
  orders$ = this.orderService.getOrders();
  filterStatus = '';

  trackById(index: number, order: Order): number {
    return order.id;
  }
}
