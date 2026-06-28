import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-return-policy',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './return-policy.component.html',
  styleUrls: ['./return-policy.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ReturnPolicyComponent {}
