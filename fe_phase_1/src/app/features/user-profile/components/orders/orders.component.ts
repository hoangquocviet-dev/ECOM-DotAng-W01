import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { OrderService, UserOrder } from '../../services/order.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent implements OnInit {
  private orderService = inject(OrderService);
  orders$: Observable<UserOrder[]> | null = null;
  
  activeTab = 'Tất cả';
  tabs = ['Tất cả', 'Chờ xử lý', 'Đang giao', 'Đã giao', 'Đã hủy'];

  ngOnInit(): void {
    this.orders$ = this.orderService.getHistory();
  }

  setTab(tab: string) {
    this.activeTab = tab;
  }
}
