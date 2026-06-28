import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { IBanner } from '../../models/banner.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminBannerService {
  private apiUrl = `${environment.apiUrl}/admin/banners`;

  constructor(private http: HttpClient) {}

  getBanners(): Observable<IBanner[]> {
    return this.http.get<IBanner[]>(this.apiUrl).pipe(
      catchError(() => {
        const mockData: IBanner[] = [
          { id: 1, imageUrl: 'https://via.placeholder.com/800x400', linkUrl: '/sale', title: 'Banner 1', isActive: true, displayOrder: 1 },
          { id: 2, imageUrl: 'https://via.placeholder.com/800x400', linkUrl: '/new', title: 'Banner 2', isActive: false, displayOrder: 2 }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  createBanner(banner: Partial<IBanner>): Observable<IBanner> {
    return this.http.post<IBanner>(this.apiUrl, banner).pipe(
      catchError(() => {
        return of({ ...banner, id: Math.floor(Math.random() * 1000) } as IBanner).pipe(delay(500));
      })
    );
  }
}
