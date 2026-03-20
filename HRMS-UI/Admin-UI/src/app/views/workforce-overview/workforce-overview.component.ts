import { Component, OnInit } from '@angular/core';
import { WorkForceOverviewService } from './workforce-overview.service';
import { CommonModule, DatePipe } from '@angular/common';
import { NgClass } from '@angular/common';
import {
  CardComponent,
  CardBodyComponent,
  CardHeaderComponent,
  CardFooterComponent,
  ProgressComponent,
  RowComponent,
  ColComponent
} from '@coreui/angular';

@Component({
  selector: 'app-workforce-overview',
  templateUrl: './workforce-overview.component.html',
  styleUrls: ['./workforce-overview.component.scss'],
  standalone: true,
  imports: [
    CommonModule,       // cho *ngFor, *ngIf, date pipe
    NgClass,            // cho [ngClass]
    DatePipe,
    CardComponent,
    CardBodyComponent,
    CardHeaderComponent,
    CardFooterComponent,
    ProgressComponent,
    RowComponent,
    ColComponent
  ],
})
export class WorkForceOverviewComponent implements OnInit {

  totalEmployees = 0;
  totalDepartments = 0;
  totalLeaves = 0;
  totalPayroll = 0;
  todayAttendance = 0;
  attendanceRate = 0;
  employees: any[] = [];

  constructor(private dashboardService: WorkForceOverviewService) { }

  ngOnInit(): void {
    this.dashboardService.getDashboard().subscribe(data => {
      this.totalEmployees = data.totalEmployees;
      this.totalDepartments = data.totalDepartments;
      this.totalLeaves = data.totalLeaves;
      this.totalPayroll = data.totalPayroll;
      this.todayAttendance = data.todayAttendance;
      this.attendanceRate = data.attendanceRate;
      this.employees = data.employees;
    });
  }
}