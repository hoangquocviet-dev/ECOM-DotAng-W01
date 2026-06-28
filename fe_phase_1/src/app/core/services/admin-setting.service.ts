import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../../environments/environment';

export interface StoreSettings {
  storeName: string;
  contactEmail: string;
  contactPhone: string;
  address: string;
  currency: string;
  maintenanceMode: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class AdminSettingService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/api/admin/settings`;

  getSettings(): Observable<StoreSettings> {
    return this.http.get<StoreSettings>(this.apiUrl).pipe(
      catchError(() => {
        return of({
          storeName: 'ECOM Fashion',
          contactEmail: 'support@ecom.com',
          contactPhone: '0123456789',
          address: '123 Đường Fashion, Quận 1, TP. HCM',
          currency: 'VND',
          maintenanceMode: false
        } as StoreSettings);
      })
    );
  }
}
