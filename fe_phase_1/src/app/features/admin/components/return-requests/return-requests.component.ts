import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminReturnRequestService } from '../../../../core/services/admin-return-request.service';
import { IReturnRequest } from '../../../../models/return-request.model';

@Component({
  selector: 'app-return-requests',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './return-requests.component.html',
  styleUrls: ['./return-requests.component.scss']
})
export class ReturnRequestsComponent implements OnInit, OnDestroy {
  requests: IReturnRequest[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  constructor(private returnRequestService: AdminReturnRequestService) {}

  ngOnInit(): void {
    this.loadRequests();
  }

  loadRequests(): void {
    this.isLoading = true;
    this.returnRequestService.getReturnRequests()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.requests = data;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading return requests', error);
          this.isLoading = false;
        }
      });
  }

  updateStatus(id: string, newStatus: 'PENDING' | 'APPROVED' | 'REJECTED' | 'COMPLETED'): void {
    this.returnRequestService.updateStatus(id, newStatus)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          const req = this.requests.find(r => r.id === id);
          if (req) {
            req.status = newStatus;
          }
          alert(`Cập nhật trạng thái thành công cho yêu cầu ${id}`);
        },
        error: (error) => {
          console.error('Error updating status', error);
          alert('Cập nhật thất bại. Vui lòng thử lại.');
        }
      });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'APPROVED': return 'badge-success';
      case 'REJECTED': return 'badge-danger';
      case 'COMPLETED': return 'badge-info';
      default: return 'badge-warning';
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
