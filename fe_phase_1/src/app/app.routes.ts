import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.routes').then(m => m.AUTH_ROUTES)
  },
  {
    path: '',
    loadComponent: () => import('./layouts/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    children: [
      // Các route khác dành cho User (Home, Products, Cart...) sẽ để ở đây
      {
        path: 'products',
        loadComponent: () => import('./features/products/components/product-list/product-list').then(m => m.ProductList)
      },
      {
        path: 'product/:slug',
        loadComponent: () => import('./features/products/components/product-detail/product-detail.component').then(m => m.ProductDetailComponent)
      },
      {
        path: 'cart',
        loadComponent: () => import('./features/checkout/components/cart/cart.component').then(m => m.CartComponent)
      },
      {
        path: 'checkout',
        loadComponent: () => import('./features/checkout/components/checkout-page/checkout-page.component').then(m => m.CheckoutPageComponent)
      },
      {
        path: 'profile',
        loadChildren: () => import('./features/user-profile/user-profile.routes').then(m => m.USER_PROFILE_ROUTES)
      },
      {
        path: '',
        loadComponent: () => import('./features/home/home.component').then(m => m.HomeComponent)
      }
    ]
  }
];
