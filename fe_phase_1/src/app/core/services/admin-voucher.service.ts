import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { IVoucher } from '../../models/voucher.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminVoucherService {
  private apiUrl = `${environment.apiUrl}/admin/vouchers`;

  constructor(private http: HttpClient) {}

  getVouchers(): Observable<IVoucher[]> {
    return this.http.get<IVoucher[]>(this.apiUrl).pipe(
      catchError(() => {
        const mockData: IVoucher[] = [
          { id: 'V01', code: 'WELCOME50', discountType: 'FIXED', discountValue: 50000, minOrderValue: 200000, startDate: new Date().toISOString(), endDate: new Date('2026-12-31').toISOString(), usageLimit: 1000, usedCount: 50, isActive: true },
          { id: 'V02', code: 'SALE20', discountType: 'PERCENTAGE', discountValue: 20, minOrderValue: 500000, maxDiscountValue: 100000, startDate: new Date().toISOString(), endDate: new Date('2026-07-31').toISOString(), usageLimit: 500, usedCount: 500, isActive: false }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  createVoucher(voucher: Partial<IVoucher>): Observable<IVoucher> {
    return this.http.post<IVoucher>(this.apiUrl, voucher).pipe(
      catchError(() => {
        return of({ ...voucher, id: `V${Math.floor(Math.random() * 1000)}`, usedCount: 0 } as IVoucher).pipe(delay(500));
      })
    );
  }

  toggleStatus(id: string, isActive: boolean): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { isActive }).pipe(
      catchError(() => {
        return of({ success: true }).pipe(delay(500));
      })
    );
  }
}
