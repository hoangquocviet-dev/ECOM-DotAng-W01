import { Component, ChangeDetectionStrategy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef, inject } from '@angular/core';

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

  isVariantModalOpen = false;
  isImagesModalOpen = false;

  variants: any[] = [];
  additionalImages: string[] = [];

  private cdr = inject(ChangeDetectorRef);

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
    console.log('Saving product:', this.product);
    console.log('Variants:', this.variants);
    console.log('Images:', this.additionalImages);
  }

  openVariantModal() {
    if (this.variants.length === 0) {
      // Mock data
      this.variants = [
        { color: 'Đỏ', size: 'M', stock: 10, priceOffset: 0, sku: 'SP-DO-M' },
        { color: 'Xanh', size: 'L', stock: 5, priceOffset: 20000, sku: 'SP-XANH-L' }
      ];
    }
    this.isVariantModalOpen = true;
  }

  closeVariantModal() {
    this.isVariantModalOpen = false;
  }

  addVariant() {
    this.variants.push({ color: '', size: '', stock: 0, priceOffset: 0, sku: '' });
    this.cdr.markForCheck();
  }

  removeVariant(index: number) {
    this.variants.splice(index, 1);
    this.cdr.markForCheck();
  }

  openImagesModal() {
    if (this.additionalImages.length === 0) {
      this.additionalImages = [
        'https://picsum.photos/100/100?random=12',
        'https://picsum.photos/100/100?random=13'
      ];
    }
    this.isImagesModalOpen = true;
  }

  closeImagesModal() {
    this.isImagesModalOpen = false;
  }

  addImage() {
    this.additionalImages.push(`https://picsum.photos/100/100?random=${Math.floor(Math.random() * 1000)}`);
    this.cdr.markForCheck();
  }

  removeImage(index: number) {
    this.additionalImages.splice(index, 1);
    this.cdr.markForCheck();
  }
}
