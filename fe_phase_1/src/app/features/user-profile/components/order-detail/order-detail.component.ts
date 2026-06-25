import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-order-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './order-detail.component.html',
  styleUrl: './order-detail.component.scss'
})
export class OrderDetailComponent implements OnInit {
  orderId: string = '';
  
  constructor(private route: ActivatedRoute) {}
  
  ngOnInit() {
    this.orderId = this.route.snapshot.paramMap.get('id') || '';
  }
}
