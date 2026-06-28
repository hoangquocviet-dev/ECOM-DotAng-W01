export interface IReturnRequest {
  id: string;
  orderId: string;
  customerName: string;
  reason: string;
  status: 'PENDING' | 'APPROVED' | 'REJECTED' | 'COMPLETED';
  dateRequested: string;
  totalAmount: number;
}
