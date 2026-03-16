import { Component, DestroyRef, DOCUMENT, effect, inject, OnInit, Renderer2, signal, WritableSignal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { ChartOptions } from 'chart.js';
import {
  AvatarComponent,
  ButtonDirective,
  ButtonGroupComponent,
  CardBodyComponent,
  CardComponent,
  CardFooterComponent,
  CardHeaderComponent,
  ColComponent,
  FormCheckLabelDirective,
  GutterDirective,
  ProgressComponent,
  RowComponent,
  TableDirective
} from '@coreui/angular';
import { ChartjsComponent } from '@coreui/angular-chartjs';
import { IconDirective } from '@coreui/icons-angular';

import {  IChartProps } from './workforce-overview-charts-data';

//
import { WorkForceOverviewService } from './workforce-overview.service';
import { CommonModule } from '@angular/common'

interface IUser {
  name: string;
  state: string;
  registered: string;
  country: string;
  usage: number;
  period: string;
  payment: string;
  activity: string;
  avatar: string;
  status: string;
  color: string;
}

interface DashboardData {
  attendance: any[];
  employees: any[];
  departments: any[];
  leaves: any[];
  salaries: any[];
}

@Component({
  standalone: true,
  templateUrl: 'workforce-overview.component.html',
  styleUrls: ['workforce-overview.component.scss'],
  imports: [ CardComponent, CardBodyComponent, RowComponent, 
    ColComponent,ReactiveFormsModule,
             ChartjsComponent, CardFooterComponent, GutterDirective, 
             ProgressComponent, CardHeaderComponent,CommonModule 
            ]
})
export class WorkForceOverviewComponent implements OnInit {

  readonly #destroyRef: DestroyRef = inject(DestroyRef);
  readonly #document: Document = inject(DOCUMENT);
  readonly #renderer: Renderer2 = inject(Renderer2);

  //
  employees: any[] = [];
  totalEmployees = 0;
  totalDepartments = 0;
  totalLeaves = 0;
  totalPayroll = 0;
  todayAttendance = 0
  attendanceRate = 100;
  departmentChartData: any[] = [];
  
  constructor(private dashboardService : WorkForceOverviewService){}

  

  public mainChart: IChartProps = { type: 'line' };
  public mainChartRef: WritableSignal<any> = signal(undefined);
  #mainChartRefEffect = effect(() => {
    if (this.mainChartRef()) {
      this.setChartStyles();
    }
  });
  public chart: Array<IChartProps> = [];
  public trafficRadioGroup = new FormGroup({
    trafficRadio: new FormControl('Month')
  });

  ngOnInit(): void {

  this.dashboardService.getDashboard()
.subscribe((res: DashboardData) => {

  console.log(res);

  // Tổng nhân viên
  this.totalEmployees = res.employees?.length || 0;

  // Tổng phòng ban
  this.totalDepartments = res.departments?.length || 0;

  // Tổng đơn nghỉ
  this.totalLeaves = res.leaves?.length || 0;

  // Số người đi làm hôm nay
  this.todayAttendance = res.attendance?.length || 0;

  // Tổng lương
  this.totalPayroll = res.salaries?.reduce(
    (sum: number, s: any) => sum + s.amount, 0
  ) || 0;

  // Attendance Rate
  this.attendanceRate = this.totalEmployees
    ? Math.round((this.todayAttendance / this.totalEmployees) * 100)
    : 0;

  // 5 nhân viên
  this.employees = res.employees?.slice(0, 5) || [];

  // Tính nhân viên theo phòng ban
  this.departmentChartData = (res.departments || []).map((d: any) => {

  const count = (res.employees || []).filter(
    (e: any) => e.departmentId === d.id
  ).length;

  return {
    name: d.departmentName,
    value: count
  };


    });

    this.mainChart = {
      type: 'bar',
      data: {
        labels: this.departmentChartData.slice(0,4).map(d => d.name),
        datasets: [
          {
            label: 'Employees by Department',
            data: this.departmentChartData.slice(0,4).map(d => d.value),
            backgroundColor: '#4f46e5'
          }
        ]
      },
      options: {
        responsive: true
      }
    };

});
    this.initCharts();
    this.updateChartOnColorModeChange();
  }

  initCharts(): void {
    this.mainChartRef()?.update();
  }
//
  setTrafficPeriod(value: string): void {
    this.trafficRadioGroup.setValue({ trafficRadio: value });
    this.initCharts();
  }

  handleChartRef($chartRef: any) {
    if ($chartRef) {
      this.mainChartRef.set($chartRef);
    }
  }

  updateChartOnColorModeChange() {
    const unListen = this.#renderer.listen(this.#document.documentElement, 'ColorSchemeChange', () => {
      this.setChartStyles();
    });

    this.#destroyRef.onDestroy(() => {
      unListen();
    });
  }

  setChartStyles() {
    if (this.mainChartRef()) {
      setTimeout(() => {
        const options: ChartOptions = { ...this.mainChart.options };
        this.mainChartRef().update();
      });
    }
  }
}
