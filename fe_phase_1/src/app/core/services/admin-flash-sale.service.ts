import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { IFlashSale } from '../../models/flash-sale.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminFlashSaleService {
  private apiUrl = `${environment.apiUrl}/admin/flash-sales`;

  constructor(private http: HttpClient) {}

  getFlashSales(): Observable<IFlashSale[]> {
    return this.http.get<IFlashSale[]>(this.apiUrl).pipe(
      catchError(() => {
        const mockData: IFlashSale[] = [
          {
            id: 1,
            name: 'Siêu Sale Sinh Nhật',
            startTime: new Date('2026-07-01T00:00:00'),
            endTime: new Date('2026-07-03T23:59:59'),
            items: []
          },
          {
            id: 2,
            name: 'Flash Sale Cuối Tuần',
            startTime: new Date('2026-06-25T00:00:00'),
            endTime: new Date('2026-06-27T23:59:59'),
            items: []
          }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  createFlashSale(fs: Partial<IFlashSale>): Observable<IFlashSale> {
    return this.http.post<IFlashSale>(this.apiUrl, fs).pipe(
      catchError(() => {
        return of({ ...fs, id: Math.floor(Math.random() * 1000) } as IFlashSale).pipe(delay(500));
      })
    );
  }
}
