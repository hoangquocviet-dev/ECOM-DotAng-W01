import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface Brand { id: number; name: string; slug: string; metaTitle: string; productCount: number; }

@Injectable({ providedIn: 'root' })
export class AdminBrandService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getBrands(): Observable<Brand[]> {
    return this.http.get<Brand[]>(`${this.apiUrl}/admin/brands`).pipe(
      catchError(err => {
        console.error('API Error, falling back to mock data:', err);
        return of([
          { id: 1, name: 'Nike', slug: 'nike', metaTitle: 'Giày Nike Chính Hãng', productCount: 120 },
          { id: 2, name: 'Adidas', slug: 'adidas', metaTitle: 'Adidas Nam Nữ', productCount: 85 },
          { id: 3, name: 'Puma', slug: 'puma', metaTitle: 'Thời trang Puma', productCount: 42 }
        ]);
      })
    );
  }
}
