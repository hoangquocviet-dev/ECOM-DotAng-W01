import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

interface Category {
  id: number;
  name: string;
  slug: string;
  metaTitle: string;
  productCount: number;
}

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoriesComponent {
  categories: Category[] = [
    { id: 1, name: 'Áo Nam', slug: 'ao-nam', metaTitle: 'Áo Nam Đẹp', productCount: 45 },
    { id: 2, name: 'Quần Nam', slug: 'quan-nam', metaTitle: 'Quần Nam Thời Trang', productCount: 32 },
    { id: 3, name: 'Phụ kiện', slug: 'phu-kien', metaTitle: 'Phụ kiện Nam Nữ', productCount: 18 }
  ];

  isModalOpen = false;
  isEditMode = false;
  currentCategory: Partial<Category> = {};

  openModal(category?: Category) {
    if (category) {
      this.isEditMode = true;
      this.currentCategory = { ...category };
    } else {
      this.isEditMode = false;
      this.currentCategory = { name: '', slug: '', metaTitle: '' };
    }
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
  }

  saveCategory() {
    // Mock save logic
    this.closeModal();
  }

  trackById(index: number, item: Category): number {
    return item.id;
  }
}
