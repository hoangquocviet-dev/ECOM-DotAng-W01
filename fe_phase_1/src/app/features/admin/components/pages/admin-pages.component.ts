import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminPageService, AdminPage } from '../../../../core/services/admin-page.service';

@Component({
  selector: 'app-admin-pages',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-pages.component.html',
  styleUrls: ['./admin-pages.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminPagesComponent {
  private pageService = inject(AdminPageService);
  pages$ = this.pageService.getPages();

  trackById(index: number, item: AdminPage): number {
    return item.id;
  }
}
