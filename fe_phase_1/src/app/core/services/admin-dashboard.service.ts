import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface StatCard { title: string; value: string; trend: number; icon: string; color: string; }
export interface TopProduct { id: number; name: string; image: string; sales: number; revenue: number; }
export interface DashboardData { stats: StatCard[]; topProducts: TopProduct[]; chartData: { label: string, value: number }[]; }

@Injectable({ providedIn: 'root' })
export class AdminDashboardService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getDashboardData(): Observable<DashboardData> {
    return this.http.get<DashboardData>(`${this.apiUrl}/admin/dashboard`).pipe(
      catchError(err => {
        console.error('API Error, falling back to mock data:', err);
        return of(this.getMockData());
      })
    );
  }

  private getMockData(): DashboardData {
    return {
      stats: [
        { title: 'Doanh thu', value: '124.5M ₫', trend: 15.2, icon: '💰', color: '#10b981' },
        { title: 'Đơn hàng mới', value: '450', trend: 8.5, icon: '📦', color: '#3b82f6' },
        { title: 'Khách hàng', value: '2,840', trend: -2.4, icon: '👥', color: '#8b5cf6' },
        { title: 'Sản phẩm', value: '1,420', trend: 4.1, icon: '👕', color: '#f59e0b' }
      ],
      topProducts: [
        { id: 1, name: 'Áo Thun Basic Cotton', image: 'https://picsum.photos/50/50?random=1', sales: 450, revenue: 45000000 },
        { id: 2, name: 'Quần Jeans Skinny', image: 'https://picsum.photos/50/50?random=2', sales: 320, revenue: 96000000 },
        { id: 3, name: 'Giày Sneaker Thể thao', image: 'https://picsum.photos/50/50?random=3', sales: 280, revenue: 140000000 },
        { id: 4, name: 'Balo Laptop Chống nước', image: 'https://picsum.photos/50/50?random=4', sales: 210, revenue: 63000000 },
        { id: 5, name: 'Mắt kính Thời trang', image: 'https://picsum.photos/50/50?random=5', sales: 180, revenue: 27000000 }
      ],
      chartData: [
        { label: 'T2', value: 45 }, { label: 'T3', value: 52 }, { label: 'T4', value: 38 },
        { label: 'T5', value: 65 }, { label: 'T6', value: 85 }, { label: 'T7', value: 110 }, { label: 'CN', value: 95 }
      ]
    };
  }
}
