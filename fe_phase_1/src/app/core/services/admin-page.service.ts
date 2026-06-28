import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface AdminPage {
  id: number;
  title: string;
  slug: string;
  status: 'Published' | 'Draft';
  updatedAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminPageService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/admin/pages`;

  getPages(): Observable<AdminPage[]> {
    return this.http.get<AdminPage[]>(this.apiUrl).pipe(
      catchError(() => {
        return of([
          { id: 1, title: 'Về chúng tôi (About Us)', slug: 'about-us', status: 'Published', updatedAt: '2026-06-20T08:00:00Z' },
          { id: 2, title: 'Chính sách bảo mật (Privacy Policy)', slug: 'privacy-policy', status: 'Published', updatedAt: '2026-06-21T09:15:00Z' },
          { id: 3, title: 'Điều khoản dịch vụ (Terms)', slug: 'terms-of-service', status: 'Draft', updatedAt: '2026-06-28T10:00:00Z' }
        ] as AdminPage[]);
      })
    );
  }
}
