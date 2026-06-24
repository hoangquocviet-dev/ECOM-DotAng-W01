import { IProduct } from './product.model';

export interface IFlashSale {
  id: number;
  name: string;
  startTime: Date;
  endTime: Date;
  items: IFlashSaleItem[];
}

export interface IFlashSaleItem {
  id: number;
  productId: number;
  product: IProduct;
  discountPercentage: number;
  discountPrice: number;
  soldQuantity: number;
  totalQuantity: number;
}
