import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-wishlist',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './wishlist.component.html',
  styleUrl: './wishlist.component.scss'
})
export class WishlistComponent {
  items = [
    { id: 1, name: 'Bút ký cao cấp', price: 250000, img: 'https://placehold.co/150' },
    { id: 2, name: 'Hộp đựng bút', price: 150000, img: 'https://placehold.co/150' }
  ];
}
