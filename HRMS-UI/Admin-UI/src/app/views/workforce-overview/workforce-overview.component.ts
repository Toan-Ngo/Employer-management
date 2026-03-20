import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { WorkForceOverviewService } from './workforce-overview.service';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { NgClass } from '@angular/common';
import {
  CardModule, GridModule, ProgressModule, TableModule, BadgeModule, SpinnerModule, WidgetModule,
  BadgeComponent
} from '@coreui/angular';
import { IconModule, IconSetService } from '@coreui/icons-angular';
import { cilPeople, cilHome, cilTask, cilMoney, cilUserPlus } from '@coreui/icons';

@Component({
  selector: 'app-workforce-overview',
  templateUrl: './workforce-overview.component.html',
  styleUrls: ['./workforce-overview.component.scss'],
  standalone: true,
  imports: [
    CommonModule, NgClass, DatePipe, DecimalPipe,
    CardModule, GridModule, ProgressModule, TableModule, 
    BadgeModule, SpinnerModule, WidgetModule, IconModule,BadgeComponent
  ],
  providers: [IconSetService]
})
export class WorkForceOverviewComponent implements OnInit {
  // Dữ liệu dashboard
  data: any = {
    totalEmployees: 0,
    totalDepartments: 0,
    totalLeaves: 0,
    totalPayroll: 0,
    todayAttendance: 0,
    attendanceRate: 0,
    employees: []
  };

  isLoading = true;

  private dashboardService = inject(WorkForceOverviewService);
  private cdr = inject(ChangeDetectorRef);
  public iconSet = inject(IconSetService);

  constructor() {
    // Đăng ký icon
    this.iconSet.icons = { cilPeople, cilHome, cilTask, cilMoney, cilUserPlus };
  }

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard() {
    this.isLoading = true;
    this.cdr.detectChanges();

    this.dashboardService.getDashboard().subscribe({
      next: (res) => {
        this.data = res;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}