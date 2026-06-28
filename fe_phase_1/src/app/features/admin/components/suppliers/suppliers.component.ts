import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminSupplierService } from '../../../../core/services/admin-supplier.service';
import { ISupplier } from '../../../../models/supplier.model';

@Component({
  selector: 'app-suppliers',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './suppliers.component.html',
  styleUrls: ['./suppliers.component.scss']
})
export class SuppliersComponent implements OnInit, OnDestroy {
  suppliers: ISupplier[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  // Modal State
  isModalOpen = false;
  isEditMode = false;
  isSaving = false;
  
  currentSupplier: Partial<ISupplier> = {};

  constructor(private supplierService: AdminSupplierService) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.isLoading = true;
    this.supplierService.getSuppliers()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.suppliers = data;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading suppliers', error);
          this.isLoading = false;
        }
      });
  }

  openAddModal(): void {
    this.isEditMode = false;
    this.currentSupplier = { isActive: true };
    this.isModalOpen = true;
  }

  openEditModal(supplier: ISupplier): void {
    this.isEditMode = true;
    this.currentSupplier = { ...supplier };
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.currentSupplier = {};
  }

  saveSupplier(): void {
    if (!this.currentSupplier.name || !this.currentSupplier.phone) return;

    this.isSaving = true;
    if (this.isEditMode && this.currentSupplier.id) {
      this.supplierService.updateSupplier(this.currentSupplier.id, this.currentSupplier)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (updated) => {
            const index = this.suppliers.findIndex(s => s.id === updated.id);
            if (index !== -1) {
              this.suppliers[index] = updated;
            }
            this.finishSaving();
          },
          error: () => this.handleError()
        });
    } else {
      this.supplierService.createSupplier(this.currentSupplier)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: (created) => {
            this.suppliers.push(created);
            this.finishSaving();
          },
          error: () => this.handleError()
        });
    }
  }

  deleteSupplier(id: string): void {
    if (confirm('Bạn có chắc chắn muốn xóa nhà cung cấp này?')) {
      this.supplierService.deleteSupplier(id)
        .pipe(takeUntil(this.destroy$))
        .subscribe({
          next: () => {
            this.suppliers = this.suppliers.filter(s => s.id !== id);
          },
          error: (error) => {
            console.error('Error deleting supplier', error);
            alert('Xóa thất bại');
          }
        });
    }
  }

  private finishSaving(): void {
    this.isSaving = false;
    this.closeModal();
    alert('Lưu thành công!');
  }

  private handleError(): void {
    this.isSaving = false;
    alert('Có lỗi xảy ra khi lưu dữ liệu!');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
