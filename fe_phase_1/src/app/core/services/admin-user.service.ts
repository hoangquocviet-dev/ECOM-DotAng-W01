import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface AdminUser {
  id: number;
  fullName: string;
  email: string;
  role: 'Admin' | 'Customer' | 'Staff';
  status: 'Active' | 'Locked';
  registeredAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminUserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/admin/users`;

  getUsers(): Observable<AdminUser[]> {
    return this.http.get<AdminUser[]>(this.apiUrl).pipe(
      catchError(() => {
        return of([
          { id: 1, fullName: 'Nguyễn Văn A', email: 'admin@ecom.com', role: 'Admin', status: 'Active', registeredAt: '2025-01-01T00:00:00Z' },
          { id: 2, fullName: 'Trần Thị B', email: 'customer1@gmail.com', role: 'Customer', status: 'Active', registeredAt: '2026-06-20T12:00:00Z' },
          { id: 3, fullName: 'Lê Văn C', email: 'staff1@ecom.com', role: 'Staff', status: 'Locked', registeredAt: '2026-05-15T09:30:00Z' }
        ] as AdminUser[]);
      })
    );
  }
}
