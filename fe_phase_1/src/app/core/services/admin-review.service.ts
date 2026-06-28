import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { IReview } from '../../models/review.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminReviewService {
  private apiUrl = `${environment.apiUrl}/admin/reviews`;

  constructor(private http: HttpClient) {}

  getReviews(): Observable<IReview[]> {
    return this.http.get<IReview[]>(this.apiUrl).pipe(
      catchError(() => {
        // Fallback mock data
        const mockData: IReview[] = [
          { id: 'RV001', productId: 'PROD001', productName: 'Áo thun nam', customerName: 'Nguyen Van A', rating: 5, comment: 'Sản phẩm rất tốt', datePosted: new Date().toISOString(), status: 'APPROVED' },
          { id: 'RV002', productId: 'PROD002', productName: 'Quần jean', customerName: 'Tran Thi B', rating: 2, comment: 'Chất vải nóng', datePosted: new Date().toISOString(), status: 'PENDING' },
          { id: 'RV003', productId: 'PROD003', productName: 'Giày thể thao', customerName: 'Le Van C', rating: 4, comment: 'Đẹp nhưng hơi chật', datePosted: new Date().toISOString(), status: 'HIDDEN', replyComment: 'Cảm ơn bạn đã góp ý' }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  updateStatus(id: string, status: 'PENDING' | 'APPROVED' | 'HIDDEN'): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}/status`, { status }).pipe(
      catchError(() => {
        return of({ success: true }).pipe(delay(500));
      })
    );
  }

  replyReview(id: string, replyComment: string): Observable<any> {
    return this.http.post(`${this.apiUrl}/${id}/reply`, { replyComment }).pipe(
      catchError(() => {
        return of({ success: true }).pipe(delay(500));
      })
    );
  }
}
