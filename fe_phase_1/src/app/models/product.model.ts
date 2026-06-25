export interface IProduct {
  id: number;
  name: string;
  slug: string;
  price: number;
  discountPrice?: number;
  imageUrl: string;
  rating?: number;
  reviewCount?: number;
  isNew?: boolean;
  category?: string;
}

export interface IVariant {
  id: number;
  color: string;
  size: string;
  price: number;
  stock: number;
}

export interface IProductDetail extends IProduct {
  images: string[];
  description: string;
  variants: IVariant[];
}
