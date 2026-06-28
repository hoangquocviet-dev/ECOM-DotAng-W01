import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { IPurchaseOrder } from '../../models/purchase-order.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminPurchaseOrderService {
  private apiUrl = `${environment.apiUrl}/admin/purchase-orders`;

  constructor(private http: HttpClient) {}

  getPurchaseOrders(): Observable<IPurchaseOrder[]> {
    return this.http.get<IPurchaseOrder[]>(this.apiUrl).pipe(
      catchError(() => {
        const mockData: IPurchaseOrder[] = [
          {
            id: 'PO001',
            supplierId: 'SUP01',
            supplierName: 'Công ty TNHH Vải Đẹp',
            orderDate: new Date().toISOString(),
            totalAmount: 15000000,
            status: 'COMPLETED',
            items: [
              { productId: 'PROD1', productName: 'Áo thun nam', quantity: 100, unitPrice: 100000, totalPrice: 10000000 },
              { productId: 'PROD2', productName: 'Quần short', quantity: 50, unitPrice: 100000, totalPrice: 5000000 }
            ]
          },
          {
            id: 'PO002',
            supplierId: 'SUP02',
            supplierName: 'Xưởng May Thời Trang',
            orderDate: new Date().toISOString(),
            totalAmount: 5000000,
            status: 'DRAFT',
            items: [
              { productId: 'PROD3', productName: 'Áo khoác', quantity: 20, unitPrice: 250000, totalPrice: 5000000 }
            ]
          }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  createPurchaseOrder(po: Partial<IPurchaseOrder>): Observable<IPurchaseOrder> {
    return this.http.post<IPurchaseOrder>(this.apiUrl, po).pipe(
      catchError(() => {
        return of({ ...po, id: `PO${Math.floor(Math.random() * 1000)}`, status: 'DRAFT' } as IPurchaseOrder).pipe(delay(500));
      })
    );
  }

  completePurchaseOrder(id: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/complete`, {}).pipe(
      catchError(() => {
        return of({ success: true }).pipe(delay(500));
      })
    );
  }
}
