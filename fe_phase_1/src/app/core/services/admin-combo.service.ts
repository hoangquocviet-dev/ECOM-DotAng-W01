import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, delay } from 'rxjs/operators';
import { ICombo } from '../../models/combo.model';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdminComboService {
  private apiUrl = `${environment.apiUrl}/admin/combos`;

  constructor(private http: HttpClient) {}

  getCombos(): Observable<ICombo[]> {
    return this.http.get<ICombo[]>(this.apiUrl).pipe(
      catchError(() => {
        const mockData: ICombo[] = [
          {
            id: 'C01',
            name: 'Combo Sinh Viên',
            description: 'Giảm giá cực sốc cho sinh viên',
            price: 150000,
            originalPrice: 200000,
            isActive: true,
            items: [
              { productId: 'P1', productName: 'Áo thun basic', quantity: 1 },
              { productId: 'P2', productName: 'Quần short', quantity: 1 }
            ]
          }
        ];
        return of(mockData).pipe(delay(500));
      })
    );
  }

  createCombo(combo: Partial<ICombo>): Observable<ICombo> {
    return this.http.post<ICombo>(this.apiUrl, combo).pipe(
      catchError(() => {
        return of({ ...combo, id: `C${Math.floor(Math.random() * 1000)}` } as ICombo).pipe(delay(500));
      })
    );
  }
}
