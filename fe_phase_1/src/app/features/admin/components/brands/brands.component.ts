import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Brand {
  id: number;
  name: string;
  slug: string;
  metaTitle: string;
  productCount: number;
}

@Component({
  selector: 'app-brands',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './brands.component.html',
  styleUrls: ['./brands.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrandsComponent {
  brands: Brand[] = [
    { id: 1, name: 'Nike', slug: 'nike', metaTitle: 'Giày Nike Chính Hãng', productCount: 120 },
    { id: 2, name: 'Adidas', slug: 'adidas', metaTitle: 'Adidas Nam Nữ', productCount: 85 },
    { id: 3, name: 'Puma', slug: 'puma', metaTitle: 'Thời trang Puma', productCount: 42 }
  ];

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
