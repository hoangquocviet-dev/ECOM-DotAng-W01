import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { AdminComboService } from '../../../../core/services/admin-combo.service';
import { ICombo } from '../../../../models/combo.model';

@Component({
  selector: 'app-combos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './combos.component.html',
  styleUrls: ['./combos.component.scss']
})
export class CombosComponent implements OnInit, OnDestroy {
  combos: ICombo[] = [];
  isLoading = true;
  private destroy$ = new Subject<void>();

  constructor(private comboService: AdminComboService) {}

  ngOnInit(): void {
    this.loadCombos();
  }

  loadCombos(): void {
    this.isLoading = true;
    this.comboService.getCombos()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (data) => {
          this.combos = data;
          this.isLoading = false;
        },
        error: () => {
          this.isLoading = false;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
