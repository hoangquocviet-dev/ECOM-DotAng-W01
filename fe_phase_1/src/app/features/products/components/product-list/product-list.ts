import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ProductService } from '../../services/product';
import { IProduct } from '../../../../models/product.model';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.scss',
})
export class ProductList implements OnInit {
  products: IProduct[] = [];
  categories = ['Tất cả', 'Sổ tay', 'Bút viết', 'Họa cụ', 'Balo', 'Giấy', 'Phụ kiện'];
  activeCategory = 'Tất cả';

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.productService.getProducts().subscribe(res => {
      this.products = res;
    });
  }

  filterByCategory(cat: string) {
    this.activeCategory = cat;
    // Client-side filter for demo
    this.productService.getProducts().subscribe(res => {
      if (cat === 'Tất cả') {
        this.products = res;
      } else {
        this.products = res.filter(p => p.category === cat);
      }
    });
  }
}
