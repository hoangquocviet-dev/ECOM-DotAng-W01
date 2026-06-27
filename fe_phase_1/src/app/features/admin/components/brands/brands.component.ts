import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminBrandService, Brand } from '../../../../core/services/admin-brand.service';

@Component({
  selector: 'app-brands',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './brands.component.html',
  styleUrls: ['./brands.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrandsComponent {
  private brandService = inject(AdminBrandService);
  brands$ = this.brandService.getBrands();

  isModalOpen = false;
  isEditMode = false;
  currentBrand: Partial<Brand> = {};

  openModal(brand?: Brand) {
    if (brand) {
      this.isEditMode = true;
      this.currentBrand = { ...brand };
    } else {
      this.isEditMode = false;
      this.currentBrand = { name: '', slug: '', metaTitle: '' };
    }
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  saveBrand() {
    // Mock save logic
    this.closeModal();
  }

  trackById(index: number, item: Brand): number {
    return item.id;
  }
}
