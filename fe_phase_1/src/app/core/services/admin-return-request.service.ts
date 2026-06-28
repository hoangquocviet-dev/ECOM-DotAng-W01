import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { IReturnRequest } from '../../models/return-request.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminReturnRequestService {
  private apiUrl = `${environment.apiUrl}/admin/return-requests`;

  constructor(private http: HttpClient) {}

  getReturnRequests(): Observable<IReturnRequest[]> {
    return this.http.get<IReturnRequest[]>(this.apiUrl).pipe(
      catchError(() => {
        // Fallback mock data
        const mockData: IReturnRequest[] = [
          { id: 'RR001', orderId: 'ORD001', customerName: 'Nguyen Van A', reason: 'Sản phẩm lỗi', status: 'PENDING', dateRequested: new Date().toISOString(), totalAmount: 500000 },
          { id: 'RR002', orderId: 'ORD002', customerName: 'Tran Thi B', reason: 'Không đúng mô tả', status: 'APPROVED', dateRequested: new Date().toISOString(), totalAmount: 200000 },
          { id: 'RR003', orderId: 'ORD003', customerName: 'Le Van C', reason: 'Đổi ý', status: 'REJECTED', dateRequested: new Date().toISOString(), totalAmount: 150000 }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  updateStatus(id: string, status: 'PENDING' | 'APPROVED' | 'REJECTED' | 'COMPLETED'): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { status }).pipe(
      catchError(() => {
        return of({ success: true }).pipe(delay(500));
      })
    );
  }
}
