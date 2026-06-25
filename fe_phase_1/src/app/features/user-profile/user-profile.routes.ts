import { Routes } from '@angular/router';

export const USER_PROFILE_ROUTES: Routes = [
  {
    path: '',
    loadComponent: () => import('./user-profile.component').then(m => m.UserProfileComponent),
    children: [
      {
        path: 'info',
        loadComponent: () => import('./components/info/info.component').then(m => m.InfoComponent)
      },
      {
        path: 'orders',
        loadComponent: () => import('./components/orders/orders.component').then(m => m.OrdersComponent)
      },
      {
        path: 'orders/:id',
        loadComponent: () => import('./components/order-detail/order-detail.component').then(m => m.OrderDetailComponent)
      },
      {
        path: 'returns',
        loadComponent: () => import('./components/returns/returns.component').then(m => m.ReturnsComponent)
      },
      {
        path: 'wishlist',
        loadComponent: () => import('./components/wishlist/wishlist.component').then(m => m.WishlistComponent)
      },
      {
        path: '',
        redirectTo: 'info',
        pathMatch: 'full'
      }
    ]
  }
];
