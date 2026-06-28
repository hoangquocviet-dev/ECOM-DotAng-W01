export interface IPurchaseOrderItem {
  productId: string;
  productName: string;
  unitPrice: number;
  quantity: number;
  totalPrice: number;
}

export interface IPurchaseOrder {
  id: string;
  supplierId: string;
  supplierName: string;
  orderDate: string;
  totalAmount: number;
  status: 'DRAFT' | 'COMPLETED' | 'CANCELLED';
  items: IPurchaseOrderItem[];
  notes?: string;
}
