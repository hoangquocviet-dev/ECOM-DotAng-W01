import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CartService } from '../../services/cart.service';
import { ICartItem } from '../../../../models/cart.model';

@Component({
  selector: 'app-cart',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss'
})
export class CartComponent implements OnInit {
  cartItems: ICartItem[] = [];
  voucherCode: string = '';
  discount: number = 0;

  constructor(private cartService: CartService) {}

  ngOnInit(): void {
    this.cartService.items$.subscribe(items => {
      this.cartItems = items;
    });
  }

  increaseQty(item: ICartItem) {
    this.cartService.updateQuantity(item.id, item.quantity + 1);
  }

  decreaseQty(item: ICartItem) {
    if (item.quantity > 1) {
      this.cartService.updateQuantity(item.id, item.quantity - 1);
    }
  }

  removeItem(item: ICartItem) {
    this.cartService.removeItem(item.id);
  }

  getSubtotal(): number {
    return this.cartService.getTotal();
  }

  getTotal(): number {
    return this.getSubtotal() - this.discount;
  }

  applyVoucher(code: string) {
    if (code.toLowerCase() === 'sale10') {
      this.discount = this.getSubtotal() * 0.1;
    } else {
      this.discount = 0;
    }
  }
}
