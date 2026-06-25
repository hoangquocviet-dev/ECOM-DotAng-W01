import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { ProductService } from '../../services/product';
import { IProductDetail, IVariant } from '../../../../models/product.model';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-detail.component.html',
  styleUrl: './product-detail.component.scss',
})
export class ProductDetailComponent implements OnInit {
  product: IProductDetail | undefined;
  activeImage: string = '';
  quantity: number = 1;
  selectedColor: string = '';
  selectedSize: string = '';

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService
  ) {}

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      if (slug) {
        this.productService.getProductBySlug(slug).subscribe(res => {
          this.product = res;
          if (this.product) {
            this.activeImage = this.product.images[0];
            if (this.product.variants && this.product.variants.length > 0) {
              this.selectedColor = this.product.variants[0].color;
              this.selectedSize = this.product.variants[0].size;
            }
          }
        });
      }
    });
  }

  changeImage(img: string) {
    this.activeImage = img;
  }

  increaseQty() {
    this.quantity++;
  }

  decreaseQty() {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  selectColor(color: string) {
    this.selectedColor = color;
  }

  selectSize(size: string) {
    this.selectedSize = size;
  }
}
