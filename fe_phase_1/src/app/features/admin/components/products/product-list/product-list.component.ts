import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

interface ProductInfo {
  id: number;
  name: string;
  image: string;
  category: string;
  brand: string;
  price: number;
  stock: number;
  status: 'Active' | 'Inactive';
}

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductListComponent {
  products: ProductInfo[] = [
    { id: 1, name: 'Áo Thun Basic', image: 'https://picsum.photos/50/50?random=11', category: 'Áo Nam', brand: 'Nike', price: 250000, stock: 150, status: 'Active' },
    { id: 2, name: 'Quần Jeans Rách', image: 'https://picsum.photos/50/50?random=12', category: 'Quần Nam', brand: 'Levi\'s', price: 450000, stock: 40, status: 'Active' },
    { id: 3, name: 'Giày Thể Thao', image: 'https://picsum.photos/50/50?random=13', category: 'Giày dép', brand: 'Adidas', price: 1200000, stock: 0, status: 'Inactive' }
  ];

  trackById(index: number, item: ProductInfo): number {
    return item.id;
  }
}
