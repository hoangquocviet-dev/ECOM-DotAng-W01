export interface IReview {
  id: string;
  productId: string;
  productName: string;
  customerName: string;
  rating: number; // 1 to 5
  comment: string;
  datePosted: string;
  status: 'PENDING' | 'APPROVED' | 'HIDDEN';
  replyComment?: string;
}
