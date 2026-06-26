import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { IBlog } from '../../models/blog.model';

@Injectable({
  providedIn: 'root'
})
export class CmsService {
  private mockBlogs: IBlog[] = [
    {
      id: '1',
      title: 'Top 10 Xu Hướng Thời Trang Hè 2026',
      slug: 'top-10-xu-huong-thoi-trang-he-2026',
      thumbnail: 'https://images.unsplash.com/photo-1515886657613-9f3515b0c78f?auto=format&fit=crop&q=80&w=800',
      summary: 'Cùng khám phá những phong cách thời trang dự kiến sẽ làm mưa làm gió trong mùa hè này.',
      content: '<p>Mùa hè 2026 đang đến gần, mang theo những xu hướng thời trang đầy màu sắc và năng động. Năm nay, sự thoải mái được đặt lên hàng đầu với các thiết kế tối giản, chất liệu thân thiện với môi trường và những gam màu pastel nhẹ nhàng.</p><h2>1. Thời Trang Tối Giản</h2><p>Phong cách tối giản tiếp tục lên ngôi...</p>',
      author: 'Admin',
      publishedDate: new Date('2026-06-25'),
      tags: ['Thời trang', 'Mùa hè']
    },
    {
      id: '2',
      title: 'Cách Phối Đồ Với Áo Thun Trắng',
      slug: 'cach-phoi-do-voi-ao-thun-trang',
      thumbnail: 'https://images.unsplash.com/photo-1521572163474-6864f9cf17ab?auto=format&fit=crop&q=80&w=800',
      summary: 'Áo thun trắng là món đồ không thể thiếu trong tủ đồ. Dưới đây là 5 cách phối đồ cực đỉnh.',
      content: '<p>Áo thun trắng không bao giờ lỗi mốt. Để mặc đẹp với áo thun trắng, bạn có thể kết hợp với quần jeans, chân váy denim, hoặc khoác thêm một chiếc blazer...</p>',
      author: 'Fashionista',
      publishedDate: new Date('2026-06-20'),
      tags: ['Phối đồ', 'Áo thun']
    },
    {
      id: '3',
      title: 'Khuyến Mãi Khủng Cuối Tuần Này',
      slug: 'khuyen-mai-khung-cuoi-tuan-nay',
      thumbnail: 'https://images.unsplash.com/photo-1607082349566-187342175e2f?auto=format&fit=crop&q=80&w=800',
      summary: 'Cơ hội săn sale lớn nhất tháng với hàng ngàn sản phẩm giảm giá đến 50%.',
      content: '<p>Đừng bỏ lỡ cơ hội mua sắm thả ga với chương trình ưu đãi giảm giá đặc biệt. Áp dụng cho tất cả các mặt hàng từ quần áo, giày dép đến phụ kiện...</p>',
      author: 'Marketing',
      publishedDate: new Date('2026-06-15'),
      tags: ['Khuyến mãi', 'Sale']
    }
  ];

  getBlogs(): Observable<IBlog[]> {
    return of(this.mockBlogs);
  }

  getBlogBySlug(slug: string): Observable<IBlog | undefined> {
    const blog = this.mockBlogs.find(b => b.slug === slug);
    return of(blog);
  }
}
