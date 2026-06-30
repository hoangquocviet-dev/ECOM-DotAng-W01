import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';

export interface OrderItem {
  id: number;
  productName: string;
  variantName: string;
  quantity: number;
  unitPrice: number;
}

export interface UserOrder {
  id: string;
  orderDate: string;
  status: string;
  totalAmount: number;
  shippingAddress: string;
  receiverName: string;
  receiverPhone: string;
  paymentMethod: string;
  shippingFee: number;
  items: OrderItem[];
}

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Orders`;

  getHistory(): Observable<UserOrder[]> {
    return this.http.get<UserOrder[]>(`${this.apiUrl}/history`).pipe(
      catchError(() => {
        return of([
          {
            id: 'ORD-1001',
            orderDate: '2026-06-25T14:30:00Z',
            status: 'Đang giao',
            totalAmount: 355000,
            shippingAddress: '456 Phố Vọng, Hai Bà Trưng, Hà Nội',
            receiverName: 'Trần Bình Trọng',
            receiverPhone: '0901234567',
            paymentMethod: 'Thanh toán khi nhận hàng (COD)',
            shippingFee: 30000,
            items: [
              { id: 1, productName: 'Sổ tay bìa da cao cấp', variantName: 'Đen / Tiêu chuẩn', quantity: 2, unitPrice: 120000 },
              { id: 2, productName: 'Bút máy luyện chữ đẹp', variantName: 'Xanh Navy', quantity: 1, unitPrice: 85000 }
            ]
          },
          {
            id: 'ORD-1002',
            orderDate: '2026-06-12T09:15:00Z',
            status: 'Đã giao',
            totalAmount: 120000,
            shippingAddress: 'Tòa nhà Bitexco, Quận 1, TP.HCM',
            receiverName: 'Trần Bình Trọng',
            receiverPhone: '0901234567',
            paymentMethod: 'Thanh toán MoMo',
            shippingFee: 15000,
            items: [
              { id: 3, productName: 'Bút bi Thiên Long', variantName: 'Xanh', quantity: 10, unitPrice: 10500 }
            ]
          }
        ]);
      })
    );
  }

  getOrderById(id: string): Observable<UserOrder | undefined> {
    return this.getHistory().pipe(
      map(orders => orders.find(o => o.id === id))
    );
  }
}
