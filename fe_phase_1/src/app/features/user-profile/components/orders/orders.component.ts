import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.scss'
})
export class OrdersComponent {
  orders = [
    { id: 'ORD-12345', date: '25/06/2026', total: 450000, status: 'Đang giao' },
    { id: 'ORD-12344', date: '12/06/2026', total: 120000, status: 'Đã giao' },
    { id: 'ORD-12343', date: '01/06/2026', total: 650000, status: 'Đã hủy' }
  ];
  activeTab = 'Tất cả';
  tabs = ['Tất cả', 'Chờ xử lý', 'Đang giao', 'Đã giao', 'Đã hủy'];

  setTab(tab: string) {
    this.activeTab = tab;
  }
}
