import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminDashboardService, StatCard, TopProduct } from '../../../../core/services/admin-dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DashboardComponent {
  private dashboardService = inject(AdminDashboardService);
  dashboardData$ = this.dashboardService.getDashboardData();

  getMaxChartValue(chartData: { label: string, value: number }[]): number {
    if (!chartData || chartData.length === 0) return 1;
    return Math.max(...chartData.map(d => d.value));
  }


  trackByTitle(index: number, item: StatCard): string {
    return item.title;
  }

  trackByLabel(index: number, item: { label: string, value: number }): string {
    return item.label;
  }

  trackById(index: number, item: TopProduct): number {
    return item.id;
  }
}
