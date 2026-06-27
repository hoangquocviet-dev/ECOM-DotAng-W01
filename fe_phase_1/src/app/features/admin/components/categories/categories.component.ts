import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminCategoryService, Category } from '../../../../core/services/admin-category.service';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CategoriesComponent {
  private categoryService = inject(AdminCategoryService);
  categories$ = this.categoryService.getCategories();

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
