import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminReviewService } from '../../../../core/services/admin-review.service';
import { IReview } from '../../../../models/review.model';

@Component({
  selector: 'app-reviews',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.scss']
})
export class ReviewsComponent implements OnInit, OnDestroy {
  reviews: IReview[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  // Reply Modal State
  isReplyModalOpen = false;
  selectedReview: IReview | null = null;
  replyText = '';
  isReplying = false;

  constructor(private reviewService: AdminReviewService) {}

  ngOnInit(): void {
    this.loadReviews();
  }

  loadReviews(): void {
    this.isLoading = true;
    this.reviewService.getReviews()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.reviews = data;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading reviews', error);
          this.isLoading = false;
        }
      });
  }

  updateStatus(id: string, newStatus: 'PENDING' | 'APPROVED' | 'HIDDEN'): void {
    this.reviewService.updateStatus(id, newStatus)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          const rev = this.reviews.find(r => r.id === id);
          if (rev) {
            rev.status = newStatus;
          }
        },
        error: (error) => {
          console.error('Error updating status', error);
          alert('Cập nhật thất bại.');
        }
      });
  }

  openReplyModal(review: IReview): void {
    this.selectedReview = review;
    this.replyText = review.replyComment || '';
    this.isReplyModalOpen = true;
  }

  closeReplyModal(): void {
    this.isReplyModalOpen = false;
    this.selectedReview = null;
    this.replyText = '';
  }

  submitReply(): void {
    if (!this.selectedReview || !this.replyText.trim()) return;

    this.isReplying = true;
    this.reviewService.replyReview(this.selectedReview.id, this.replyText)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          const rev = this.reviews.find(r => r.id === this.selectedReview?.id);
          if (rev) {
            rev.replyComment = this.replyText;
            rev.status = 'APPROVED'; // Auto approve when replied
          }
          this.isReplying = false;
          this.closeReplyModal();
          alert('Phản hồi thành công!');
        },
        error: (error) => {
          console.error('Error replying', error);
          this.isReplying = false;
          alert('Có lỗi xảy ra.');
        }
      });
  }

  getStatusClass(status: string): string {
    switch (status) {
      case 'APPROVED': return 'badge-success';
      case 'HIDDEN': return 'badge-danger';
      default: return 'badge-warning';
    }
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
