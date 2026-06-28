import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface AdminBlog {
  id: number;
  title: string;
  slug: string;
  author: string;
  status: 'Published' | 'Draft';
  createdAt: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminBlogService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/admin/blogs`;

  getBlogs(): Observable<AdminBlog[]> {
    return this.http.get<AdminBlog[]>(this.apiUrl).pipe(
      catchError(() => {
        // Fallback mock data
        return of([
          { id: 1, title: 'Hướng dẫn phối đồ mùa hè 2026', slug: 'phoi-do-mua-he', author: 'Admin', status: 'Published', createdAt: '2026-06-25T10:00:00Z' },
          { id: 2, title: 'Top 5 xu hướng thời trang năm nay', slug: 'xu-huong-thoi-trang', author: 'Editor', status: 'Draft', createdAt: '2026-06-26T14:30:00Z' }
        ] as AdminBlog[]);
      })
    );
  }
}
