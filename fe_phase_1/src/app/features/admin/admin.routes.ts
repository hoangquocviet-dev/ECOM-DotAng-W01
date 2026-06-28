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
        path: 'flash-sales',
        loadComponent: () => import('./components/flash-sales/flash-sales.component').then(m => m.FlashSalesComponent)
      },
      {
        path: 'vouchers',
        loadComponent: () => import('./components/vouchers/vouchers.component').then(m => m.VouchersComponent)
      },
      {
        path: 'combos',
        loadComponent: () => import('./components/combos/combos.component').then(m => m.CombosComponent)
      },
      {
        path: 'banners',
        loadComponent: () => import('./components/banners/banners.component').then(m => m.BannersComponent)
      },
      {
        path: 'blogs',
        loadComponent: () => import('./components/blogs/admin-blogs.component').then(m => m.AdminBlogsComponent)
      },
      {
        path: 'pages',
        loadComponent: () => import('./components/pages/admin-pages.component').then(m => m.AdminPagesComponent)
      },
      {
        path: 'users',
        loadComponent: () => import('./components/users/admin-users.component').then(m => m.AdminUsersComponent)
      },
      {
        path: 'settings',
        loadComponent: () => import('./components/settings/admin-settings.component').then(m => m.AdminSettingsComponent)
      },
      {
        path: '',
        redirectTo: 'dashboard',
        pathMatch: 'full'
      }
    ]
  }
];
