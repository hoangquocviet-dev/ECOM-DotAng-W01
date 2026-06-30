import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../../environments/environment';

export interface UserProfile {
  id: number;
  username: string;
  email: string;
  name: string;
  phoneNumber: string;
  address: string;
  role: string;
  totalSpent: number;
  rewardPoints: number;
  memberTier: string;
  referralCode: string;
  referredById: number | null;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Users`;

  getProfile(): Observable<UserProfile> {
    return this.http.get<UserProfile>(`${this.apiUrl}/profile`).pipe(
      catchError(() => {
        // Fallback Mock Data
        return of({
          id: 1,
          username: 'nguyenvana',
          email: 'nguyenvana@gmail.com',
          name: 'Nguyễn Văn A',
          phoneNumber: '0987654321',
          address: '123 Đường Số 1, Quận 1, TP.HCM',
          role: 'User',
          totalSpent: 10000000,
          rewardPoints: 1500,
          memberTier: 'VIP',
          referralCode: 'REF-NGUYENVANA123',
          referredById: null
        });
      })
    );
  }

  updateProfile(data: any): Observable<UserProfile> {
    return this.http.put<UserProfile>(`${this.apiUrl}/profile`, data);
  }

  changePassword(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/change-password`, data);
  }
}
