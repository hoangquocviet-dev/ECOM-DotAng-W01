import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './product-form.component.html',
  styleUrls: ['./product-form.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductFormComponent implements OnInit {
  isEditMode = false;
  productId: string | null = null;

  product = {
    name: '',
    categoryId: '',
    brandId: '',
    price: 0,
    mainImage: '',
    description: '',
    lowStockThreshold: 10,
    slug: '',
    metaTitle: '',
    metaDescription: ''
  };

  constructor(private route: ActivatedRoute) {}

  ngOnInit() {
    this.productId = this.route.snapshot.paramMap.get('id');
    if (this.productId) {
      this.isEditMode = true;
      // Mock loading data
      this.product = {
        name: 'Sản phẩm mẫu sửa',
        categoryId: '1',
        brandId: '1',
        price: 250000,
        mainImage: 'https://picsum.photos/500/500?random=11',
        description: '<p>Mô tả chi tiết sản phẩm...</p>',
        lowStockThreshold: 5,
        slug: 'san-pham-mau-sua',
        metaTitle: 'Sản phẩm mẫu',
        metaDescription: 'Mô tả ngắn'
      };
    }
  }

  saveProduct() {
    // Mock save logic
    console.log('Saving product:', this.product);
  }
}
