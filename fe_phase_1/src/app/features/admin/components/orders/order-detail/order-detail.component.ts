import { Component, ChangeDetectionStrategy, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { AdminOrderService, Order } from '../../../../../core/services/admin-order.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './order-detail.component.html',
  styleUrls: ['./order-detail.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class OrderDetailComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private orderService = inject(AdminOrderService);
  
  order$!: Observable<Order>;

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.order$ = this.orderService.getOrderDetails(id);
    }
  }

  updateStatus(id: number, status: string) {
    this.orderService.updateStatus(id, status).subscribe(() => {
      alert('Đã cập nhật trạng thái đơn hàng thành: ' + status);
      // Ideally, trigger a refresh of order$ here via a BehaviorSubject or refetch.
      this.order$ = this.orderService.getOrderDetails(id);
    });
  }

  exportPDF(orderCode: string) {
    alert(`Đang xuất PDF hóa đơn cho đơn hàng ${orderCode}...`);
  }
}
