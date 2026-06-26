import { Routes } from '@angular/router';

export const CMS_ROUTES: Routes = [
  {
    path: 'blogs',
    loadComponent: () => import('./blogs/blogs.component').then(m => m.BlogsComponent)
  },
  {
    path: 'blog/:slug',
    loadComponent: () => import('./blog-detail/blog-detail.component').then(m => m.BlogDetailComponent)
  },
  {
    path: 'about-us',
    loadComponent: () => import('./about-us/about-us.component').then(m => m.AboutUsComponent)
  },
  {
    path: 'privacy-policy',
    loadComponent: () => import('./privacy-policy/privacy-policy.component').then(m => m.PrivacyPolicyComponent)
  }
];
