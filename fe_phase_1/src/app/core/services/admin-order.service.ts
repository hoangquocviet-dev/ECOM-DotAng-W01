import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface OrderItem {
  id: number;
  productName: string;
  image: string;
  price: number;
  quantity: number;
  total: number;
}

export interface Order {
  id: number;
  orderCode: string;
  customerName: string;
  customerEmail: string;
  customerPhone: string;
  shippingAddress: string;
  orderDate: Date;
  totalAmount: number;
  paymentStatus: 'Paid' | 'Unpaid';
  paymentMethod: 'COD' | 'MOMO';
  orderStatus: 'Pending' | 'Processing' | 'Shipping' | 'Completed' | 'Cancelled';
  items?: OrderItem[];
}

@Injectable({ providedIn: 'root' })
export class AdminOrderService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.apiUrl}/admin/orders`).pipe(
      catchError(err => {
        console.error('API Error, falling back to mock data:', err);
        return of(this.getMockOrders());
      })
    );
  }

  getOrderDetails(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.apiUrl}/admin/orders/${id}`).pipe(
      catchError(err => {
        console.error('API Error, falling back to mock data:', err);
        const order = this.getMockOrders().find(o => o.id === id);
        return of(order || this.getMockOrders()[0]);
      })
    );
  }
  
  updateOrderStatus(id: number, status: string): Observable<boolean> {
     // Mock update
     return of(true);
  }

  private getMockOrders(): Order[] {
    return [
      { id: 1, orderCode: 'ORD-2023-001', customerName: 'Nguyễn Văn A', customerEmail: 'nguyenvana@gmail.com', customerPhone: '0901234567', shippingAddress: '123 Đường Số 1, Quận 1, TP.HCM', orderDate: new Date('2023-10-25T10:30:00'), totalAmount: 1450000, paymentStatus: 'Paid', paymentMethod: 'MOMO', orderStatus: 'Completed', items: [{ id: 1, productName: 'Áo Thun Basic', image: 'https://picsum.photos/50', price: 250000, quantity: 2, total: 500000 }, { id: 2, productName: 'Quần Jeans', image: 'https://picsum.photos/50', price: 950000, quantity: 1, total: 950000 }] },
      { id: 2, orderCode: 'ORD-2023-002', customerName: 'Trần Thị B', customerEmail: 'tranthib@gmail.com', customerPhone: '0987654321', shippingAddress: '456 Lê Lợi, Quận Hải Châu, Đà Nẵng', orderDate: new Date('2023-10-26T14:15:00'), totalAmount: 350000, paymentStatus: 'Unpaid', paymentMethod: 'COD', orderStatus: 'Pending', items: [{ id: 3, productName: 'Nón Kết Nam', image: 'https://picsum.photos/50', price: 350000, quantity: 1, total: 350000 }] },
      { id: 3, orderCode: 'ORD-2023-003', customerName: 'Lê Văn C', customerEmail: 'levanc@gmail.com', customerPhone: '0912345678', shippingAddress: '789 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội', orderDate: new Date('2023-10-27T09:00:00'), totalAmount: 2500000, paymentStatus: 'Paid', paymentMethod: 'MOMO', orderStatus: 'Processing', items: [{ id: 4, productName: 'Giày Thể Thao', image: 'https://picsum.photos/50', price: 2500000, quantity: 1, total: 2500000 }] }
    ];
  }
}
