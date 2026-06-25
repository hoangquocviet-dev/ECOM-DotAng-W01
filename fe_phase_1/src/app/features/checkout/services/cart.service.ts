import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ICartItem } from '../../../models/cart.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private itemsSubject = new BehaviorSubject<ICartItem[]>([]);
  public items$ = this.itemsSubject.asObservable();

  constructor() {
    // Demo data
    this.itemsSubject.next([
      {
        id: '1',
        product: { id: 1, name: 'Sổ tay bìa da cao cấp', slug: 'so-tay', price: 150000, discountPrice: 120000, imageUrl: 'https://placehold.co/100x100', category: 'Sổ tay' },
        variant: { id: 1, color: 'Đen', size: 'Tiêu chuẩn', price: 120000, stock: 50 },
        quantity: 2,
        totalPrice: 240000
      },
      {
        id: '2',
        product: { id: 2, name: 'Bút máy luyện chữ đẹp', slug: 'but-may', price: 85000, imageUrl: 'https://placehold.co/100x100', category: 'Bút' },
        quantity: 1,
        totalPrice: 85000
      }
    ]);
  }

  getItems(): ICartItem[] {
    return this.itemsSubject.getValue();
  }

  updateQuantity(id: string, qty: number) {
    const items = this.getItems();
    const item = items.find(i => i.id === id);
    if (item) {
      item.quantity = qty;
      const price = item.variant?.price || item.product.discountPrice || item.product.price;
      item.totalPrice = price * qty;
      this.itemsSubject.next([...items]);
    }
  }

  removeItem(id: string) {
    const items = this.getItems().filter(i => i.id !== id);
    this.itemsSubject.next(items);
  }

  getTotal(): number {
    return this.getItems().reduce((sum, item) => sum + item.totalPrice, 0);
  }
}
