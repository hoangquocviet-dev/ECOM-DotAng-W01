import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface ProductInfo { id: number; name: string; image: string; category: string; brand: string; price: number; stock: number; status: 'Active' | 'Inactive'; }

@Injectable({ providedIn: 'root' })
export class AdminProductService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getProducts(): Observable<ProductInfo[]> {
    return this.http.get<ProductInfo[]>(`${this.apiUrl}/admin/products`).pipe(
      catchError(err => {
        console.error('API Error, falling back to mock data:', err);
        return of([
          { id: 1, name: 'Áo Thun Basic', image: 'https://picsum.photos/50/50?random=11', category: 'Áo Nam', brand: 'Nike', price: 250000, stock: 150, status: 'Active' },
          { id: 2, name: 'Quần Jeans Rách', image: 'https://picsum.photos/50/50?random=12', category: 'Quần Nam', brand: 'Levi\'s', price: 450000, stock: 40, status: 'Active' },
          { id: 3, name: 'Giày Thể Thao', image: 'https://picsum.photos/50/50?random=13', category: 'Giày dép', brand: 'Adidas', price: 1200000, stock: 0, status: 'Inactive' }
        ] as ProductInfo[]);
      })
    );
  }
}
