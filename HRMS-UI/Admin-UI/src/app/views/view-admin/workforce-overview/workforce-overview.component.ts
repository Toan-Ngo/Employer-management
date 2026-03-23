import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router'; // Thêm Router
import { WorkForceOverviewService } from './workforce-overview.service';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { NgClass } from '@angular/common';
import {
  CardModule,
  GridModule,
  ProgressModule,
  TableModule,
  BadgeModule,
  SpinnerModule,
  WidgetModule,
  ButtonModule, // Thêm ButtonModule
} from '@coreui/angular';
import { IconModule, IconSetService } from '@coreui/icons-angular';
import {
  cilPeople,
  cilHome,
  cilTask,
  cilMoney,
  cilUserPlus,
} from '@coreui/icons';

@Component({
  selector: 'app-workforce-overview',
  templateUrl: './workforce-overview.component.html',
  styleUrls: ['./workforce-overview.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    NgClass,
    DatePipe,
    DecimalPipe,
    CardModule,
    GridModule,
    ProgressModule,
    TableModule,
    BadgeModule,
    SpinnerModule,
    WidgetModule,
    IconModule,
    ButtonModule,
  ],
  providers: [IconSetService],
})
export class WorkForceOverviewComponent implements OnInit {
  data: any = { stats: [], attendanceRate: 0, employees: [] };
  isLoading = true;

  private dashboardService = inject(WorkForceOverviewService);
  private cdr = inject(ChangeDetectorRef);
  private router = inject(Router); // Inject Router
  public iconSet = inject(IconSetService);

  constructor() {
    this.iconSet.icons = { cilPeople, cilHome, cilTask, cilMoney, cilUserPlus };
  }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {
    this.isLoading = true;
    this.dashboardService.getDashboard().subscribe({
      next: (res) => {
        this.data = res;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      },
    });
  }

  // Hàm xử lý nút Xem tất cả
  viewAllEmployees() {
    this.router.navigate(['/employees']); // Thay đổi path đúng với route của bạn
  }
}
