import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { environment } from '../../../environments/environment';
import { IBanner } from '../../models/banner.model';
import { IProduct } from '../../models/product.model';
import { IFlashSale } from '../../models/flash-sale.model';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  private http = inject(HttpClient);
  private apiUrl = environment.apiUrl;

  getBanners(): Observable<IBanner[]> {
    // Tạm thời trả về mock data để dựng UI
    return of([
      { id: 1, imageUrl: 'https://placehold.co/1200x400/007bff/fff?text=M%C3%B9a+T%E1%BB%B1u+Tr%C6%B0%E1%BB%9Dng', linkUrl: '/shop', title: 'Mùa Tựu Trường', isActive: true, displayOrder: 1 },
      { id: 2, imageUrl: 'https://placehold.co/1200x400/ff6b6b/fff?text=Flash+Sale', linkUrl: '/flash-sale', title: 'Flash Sale', isActive: true, displayOrder: 2 },
      { id: 3, imageUrl: 'https://placehold.co/1200x400/20c997/fff?text=S%E1%BA%A3n+Ph%E1%BA%A9m+M%E1%BB%9Bi', linkUrl: '/shop', title: 'Sản Phẩm Mới', isActive: true, displayOrder: 3 }
    ]);
    // Thực tế: return this.http.get<IBanner[]>(`${this.apiUrl}/Banners/active`);
  }

  getFeaturedProducts(): Observable<IProduct[]> {
    return of([
      { id: 1, name: 'Bút Bi Thiên Long', price: 5000, imageUrl: 'https://placehold.co/300x300/eee/333?text=B%C3%BAt', rating: 4.5, reviewCount: 120, isNew: true },
      { id: 2, name: 'Vở Hồng Hà 96 Trang', price: 12000, imageUrl: 'https://placehold.co/300x300/eee/333?text=V%E1%BB%9F', rating: 4.8, reviewCount: 85 },
      { id: 3, name: 'Hộp Bút Deli', price: 45000, discountPrice: 35000, imageUrl: 'https://placehold.co/300x300/eee/333?text=H%E1%BB%99p+b%C3%BAt', rating: 4.0, reviewCount: 40 },
      { id: 4, name: 'Balo Đi Học Chống Nước', price: 250000, discountPrice: 199000, imageUrl: 'https://placehold.co/300x300/eee/333?text=Balo', rating: 4.9, reviewCount: 300, isNew: true },
      { id: 5, name: 'Thước Kẻ 20cm', price: 3000, imageUrl: 'https://placehold.co/300x300/eee/333?text=Th%C6%B0%E1%BB%9Bc', rating: 4.2, reviewCount: 15 }
    ]);
  }

  getFlashSale(): Observable<IFlashSale | null> {
    const end = new Date();
    end.setHours(end.getHours() + 5); // Kết thúc sau 5 giờ
    
    return of({
      id: 1,
      name: 'Siêu Sale Giờ Vàng',
      startTime: new Date(),
      endTime: end,
      items: [
        { id: 1, productId: 3, product: { id: 3, name: 'Hộp Bút Deli', price: 45000, imageUrl: 'https://placehold.co/300x300/eee/333?text=H%E1%BB%99p+b%C3%BAt' }, discountPercentage: 50, discountPrice: 22500, soldQuantity: 80, totalQuantity: 100 },
        { id: 2, productId: 4, product: { id: 4, name: 'Balo Đi Học Chống Nước', price: 250000, imageUrl: 'https://placehold.co/300x300/eee/333?text=Balo' }, discountPercentage: 30, discountPrice: 175000, soldQuantity: 45, totalQuantity: 50 }
      ]
    });
  }
}
