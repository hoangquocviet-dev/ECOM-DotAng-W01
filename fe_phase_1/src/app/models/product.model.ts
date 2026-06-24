export interface IProduct {
  id: number;
  name: string;
  slug?: string;
  price: number;
  discountPrice?: number;
  imageUrl: string;
  rating?: number;
  reviewCount?: number;
  isNew?: boolean;
}
