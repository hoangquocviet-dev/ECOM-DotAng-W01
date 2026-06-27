import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface Category { id: number; name: string; slug: string; metaTitle: string; productCount: number; }

@Injectable({ providedIn: 'root' })
export class AdminCategoryService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getCategories(): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.apiUrl}/admin/categories`).pipe(
      catchError(err => {
        console.error('API Error, falling back to mock data:', err);
        return of([
          { id: 1, name: 'Áo Nam', slug: 'ao-nam', metaTitle: 'Áo Nam Đẹp', productCount: 45 },
          { id: 2, name: 'Quần Nam', slug: 'quan-nam', metaTitle: 'Quần Nam Thời Trang', productCount: 32 },
          { id: 3, name: 'Phụ kiện', slug: 'phu-kien', metaTitle: 'Phụ kiện Nam Nữ', productCount: 18 }
        ]);
      })
    );
  }
}
