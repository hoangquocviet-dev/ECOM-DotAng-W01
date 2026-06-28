import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminSettingService } from '../../../../core/services/admin-setting.service';

@Component({
  selector: 'app-admin-settings',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-settings.component.html',
  styleUrls: ['./admin-settings.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AdminSettingsComponent {
  private settingService = inject(AdminSettingService);
  settings$ = this.settingService.getSettings();

  saveSettings() {
    // Mock save
    alert('Cập nhật cấu hình thành công!');
  }
}
