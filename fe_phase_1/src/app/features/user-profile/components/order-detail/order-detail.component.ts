import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { OrderService, UserOrder } from '../../services/order.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.scss'
})
export class OrderDetailComponent implements OnInit {
  orderId: string = '';
  order$: Observable<UserOrder | undefined> | null = null;
  
  private route = inject(ActivatedRoute);
  private orderService = inject(OrderService);
  
  ngOnInit() {
    this.orderId = this.route.snapshot.paramMap.get('id') || '';
    if (this.orderId) {
      this.order$ = this.orderService.getOrderById(this.orderId);
    }
  }
}
