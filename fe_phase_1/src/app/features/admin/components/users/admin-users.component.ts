import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminUserService, AdminUser } from '../../../../core/services/admin-user.service';

@Component({
  selector: 'app-admin-users',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-users.component.html',
  styleUrls: ['./admin-users.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminUsersComponent {
  private userService = inject(AdminUserService);
  users$ = this.userService.getUsers();

  trackById(index: number, item: AdminUser): number {
    return item.id;
  }
}
