import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IProduct, IProductDetail } from '../../../models/product.model';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private dummyProducts: IProduct[] = [
    { id: 1, name: 'Sổ tay bìa da cao cấp', slug: 'so-tay-bia-da-cao-cap', price: 150000, discountPrice: 120000, imageUrl: 'https://placehold.co/400x400/2a2a2a/ffffff?text=So+Tay', rating: 4.8, reviewCount: 124, isNew: true, category: 'Sổ tay' },
    { id: 2, name: 'Bút máy luyện chữ đẹp', slug: 'but-may-luyen-chu-dep', price: 85000, imageUrl: 'https://placehold.co/400x400/2a2a2a/ffffff?text=But+May', rating: 4.5, reviewCount: 89, category: 'Bút viết' },
    { id: 3, name: 'Bộ màu nước 24 màu', slug: 'bo-mau-nuoc-24-mau', price: 320000, discountPrice: 299000, imageUrl: 'https://placehold.co/400x400/2a2a2a/ffffff?text=Mau+Nuoc', rating: 4.9, reviewCount: 450, category: 'Họa cụ' },
    { id: 4, name: 'Balo laptop chống nước', slug: 'balo-laptop-chong-nuoc', price: 550000, imageUrl: 'https://placehold.co/400x400/2a2a2a/ffffff?text=Balo', rating: 4.7, reviewCount: 56, isNew: true, category: 'Balo' },
    { id: 5, name: 'Giấy vẽ phác thảo A4', slug: 'giay-ve-phac-thao-a4', price: 45000, imageUrl: 'https://placehold.co/400x400/2a2a2a/ffffff?text=Giay+A4', rating: 4.6, reviewCount: 230, category: 'Giấy' },
    { id: 6, name: 'Hộp bút vải Canvas', slug: 'hop-but-vai-canvas', price: 65000, discountPrice: 50000, imageUrl: 'https://placehold.co/400x400/2a2a2a/ffffff?text=Hop+But', rating: 4.3, reviewCount: 78, category: 'Phụ kiện' }
  ];

  getProducts(): Observable<IProduct[]> {
    return of(this.dummyProducts);
  }

  getProductBySlug(slug: string): Observable<IProductDetail | undefined> {
    const p = this.dummyProducts.find(x => x.slug === slug);
    if (!p) return of(undefined);

    const detail: IProductDetail = {
      ...p,
      images: [
        p.imageUrl,
        'https://placehold.co/400x400/333333/ffffff?text=Goc+1',
        'https://placehold.co/400x400/444444/ffffff?text=Goc+2',
        'https://placehold.co/400x400/555555/ffffff?text=Goc+3',
      ],
      description: '<p>Sản phẩm chất lượng cao, phù hợp cho học sinh, sinh viên và dân văn phòng.</p><ul><li>Chất liệu cao cấp</li><li>Thiết kế hiện đại</li><li>Độ bền cao</li></ul>',
      variants: [
        { id: 1, color: 'Đen', size: 'Tiêu chuẩn', price: p.price, stock: 50 },
        { id: 2, color: 'Trắng', size: 'Tiêu chuẩn', price: p.price, stock: 20 },
      ]
    };
    return of(detail);
  }
}
