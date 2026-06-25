import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-returns',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './returns.component.html',
  styleUrl: './returns.component.scss'
})
export class ReturnsComponent {
  returns = [
    { id: 'RET-001', orderId: 'ORD-12345', date: '26/06/2026', reason: 'Hàng lỗi kỹ thuật', status: 'Đang xử lý' }
  ];
}
