import { IProduct } from './product.model';

export interface IComboItem {
  productId: string;
  productName: string;
  quantity: number;
}

export interface ICombo {
  id: string;
  name: string;
  description: string;
  price: number;
  originalPrice: number;
  isActive: boolean;
  items: IComboItem[];
}
