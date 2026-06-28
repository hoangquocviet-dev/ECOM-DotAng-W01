import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminPurchaseOrderService } from '../../../../../core/services/admin-purchase-order.service';
import { AdminSupplierService } from '../../../../../core/services/admin-supplier.service';
import { IPurchaseOrder, IPurchaseOrderItem } from '../../../../../models/purchase-order.model';
import { ISupplier } from '../../../../../models/supplier.model';

@Component({
  selector: 'app-purchase-order-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './purchase-order-form.component.html',
  styleUrls: ['./purchase-order-form.component.scss']
})
export class PurchaseOrderFormComponent implements OnInit, OnDestroy {
  suppliers: ISupplier[] = [];
  selectedSupplierId = '';
  
  items: IPurchaseOrderItem[] = [];
  notes = '';
  isSaving = false;

  // New item form
  newItem: Partial<IPurchaseOrderItem> = {};

  private destroy$ = new Subject<void>();

  constructor(
    private poService: AdminPurchaseOrderService,
    private supplierService: AdminSupplierService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.supplierService.getSuppliers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.suppliers = data.filter(s => s.isActive);
        }
      });
  }

  addItem(): void {
    if (!this.newItem.productName || !this.newItem.quantity || !this.newItem.unitPrice) return;

    this.items.push({
      productId: `PROD${Math.floor(Math.random() * 1000)}`, // Mock product ID
      productName: this.newItem.productName,
      quantity: this.newItem.quantity,
      unitPrice: this.newItem.unitPrice,
      totalPrice: this.newItem.quantity * this.newItem.unitPrice
    });

    this.newItem = {}; // reset
  }

  removeItem(index: number): void {
    this.items.splice(index, 1);
  }

  get totalAmount(): number {
    return this.items.reduce((sum, item) => sum + item.totalPrice, 0);
  }

  createOrder(): void {
    if (!this.selectedSupplierId || this.items.length === 0) return;

    this.isSaving = true;
    const supplier = this.suppliers.find(s => s.id === this.selectedSupplierId);
    
    const newOrder: Partial<IPurchaseOrder> = {
      supplierId: this.selectedSupplierId,
      supplierName: supplier?.name,
      orderDate: new Date().toISOString(),
      items: this.items,
      totalAmount: this.totalAmount,
      notes: this.notes
    };

    this.poService.createPurchaseOrder(newOrder)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: () => {
          alert('Tạo Phiếu Nhập Kho thành công!');
          this.router.navigate(['/admin/purchase-orders']);
        },
        error: () => {
          alert('Có lỗi xảy ra!');
          this.isSaving = false;
        }
      });
  }
  
  cancel(): void {
    this.router.navigate(['/admin/purchase-orders']);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
