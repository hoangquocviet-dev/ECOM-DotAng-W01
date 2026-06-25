import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { ICartItem } from '../../../../models/cart.model';

@Component({
  selector: 'app-checkout-page',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './checkout-page.component.html',
  styleUrl: './checkout-page.component.scss'
})
export class CheckoutPageComponent implements OnInit {
  cartItems: ICartItem[] = [];
  shippingFee: number = 30000;
  paymentMethod: string = 'cod';

  constructor(
    private cartService: CartService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.cartItems = this.cartService.getItems();
    if (this.cartItems.length === 0) {
      this.router.navigate(['/cart']);
    }
  }

  getSubtotal(): number {
    return this.cartService.getTotal();
  }

  getTotal(): number {
    return this.getSubtotal() + this.shippingFee;
  }

  setPaymentMethod(method: string) {
    this.paymentMethod = method;
  }

  placeOrder() {
    alert('Đặt hàng thành công!');
    this.router.navigate(['/']);
  }
}
