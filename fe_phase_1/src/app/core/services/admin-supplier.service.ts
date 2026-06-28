import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { ISupplier } from '../../models/supplier.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminSupplierService {
  private apiUrl = `${environment.apiUrl}/admin/suppliers`;

  constructor(private http: HttpClient) {}

  getSuppliers(): Observable<ISupplier[]> {
    return this.http.get<ISupplier[]>(this.apiUrl).pipe(
      catchError(() => {
        const mockData: ISupplier[] = [
          { id: 'SUP01', name: 'Công ty TNHH Vải Đẹp', address: '123 Đường A, HCM', phone: '0901234567', email: 'vaidep@gmail.com', contactPerson: 'Nguyễn Văn A', isActive: true },
          { id: 'SUP02', name: 'Xưởng May Thời Trang', address: '456 Đường B, Hà Nội', phone: '0987654321', email: 'xuongmay@gmail.com', contactPerson: 'Trần Thị B', isActive: true }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  createSupplier(supplier: Partial<ISupplier>): Observable<ISupplier> {
    return this.http.post<ISupplier>(this.apiUrl, supplier).pipe(
      catchError(() => {
        return of({ ...supplier, id: `SUP${Math.floor(Math.random() * 1000)}` } as ISupplier).pipe(delay(500));
      })
    );
  }

  updateSupplier(id: string, supplier: Partial<ISupplier>): Observable<ISupplier> {
    return this.http.put<ISupplier>(`${this.apiUrl}/${id}`, supplier).pipe(
      catchError(() => {
        return of({ ...supplier, id } as ISupplier).pipe(delay(500));
      })
    );
  }

  deleteSupplier(id: string): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`).pipe(
      catchError(() => {
        return of({ success: true }).pipe(delay(500));
      })
    );
  }
}
