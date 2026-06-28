import { Routes } from '@angular/router';

export const ADMIN_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./components/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./components/dashboard/dashboard.component').then(m => m.DashboardComponent)
      },
      {
        path: 'orders',
        loadComponent: () => import('./components/orders/order-list/order-list.component').then(m => m.OrderListComponent)
      },
      {
        path: 'orders/:id',
        loadComponent: () => import('./components/orders/order-detail/order-detail.component').then(m => m.OrderDetailComponent)
      },
      {
        path: 'categories',
        loadComponent: () => import('./components/categories/categories.component').then(m => m.CategoriesComponent)
      },
      {
        path: 'brands',
        loadComponent: () => import('./components/brands/brands.component').then(m => m.BrandsComponent)
      },
      {
        path: 'products',
        loadComponent: () => import('./components/products/product-list/product-list.component').then(m => m.ProductListComponent)
      },
      {
        path: 'products/create',
        loadComponent: () => import('./components/products/product-form/product-form.component').then(m => m.ProductFormComponent)
      },
      {
        path: 'products/edit/:id',
        loadComponent: () => import('./components/products/product-form/product-form.component').then(m => m.ProductFormComponent)
      },
      {
        path: 'return-requests',
        loadComponent: () => import('./components/return-requests/return-requests.component').then(m => m.ReturnRequestsComponent)
      },
      {
        path: 'reviews',
        loadComponent: () => import('./components/reviews/reviews.component').then(m => m.ReviewsComponent)
      },
      {
        path: 'suppliers',
        loadComponent: () => import('./components/suppliers/suppliers.component').then(m => m.SuppliersComponent)
      },
      {
        path: 'purchase-orders',
        loadComponent: () => import('./components/purchase-orders/purchase-order-list/purchase-order-list.component').then(m => m.PurchaseOrderListComponent)
      },
      {
        path: 'purchase-orders/create',
        loadComponent: () => import('./components/purchase-orders/purchase-order-form/purchase-order-form.component').then(m => m.PurchaseOrderFormComponent)
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  }
];
