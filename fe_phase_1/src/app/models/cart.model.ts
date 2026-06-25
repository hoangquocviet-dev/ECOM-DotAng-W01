import { IProduct, IVariant } from './product.model';

export interface ICartItem {
  id: string;
  product: IProduct;
  variant?: IVariant;
  quantity: number;
  totalPrice: number;
}
