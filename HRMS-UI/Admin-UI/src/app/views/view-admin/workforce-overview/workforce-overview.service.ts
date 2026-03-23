import { Injectable } from '@angular/core';
import { forkJoin, of, map, catchError } from 'rxjs';
import {
  EmployeeApiClient,
  DepartmentApiClient,
  LeaveRequestApiClient,
  SalaryApiClient,
  AttendanceApiClient,
} from '../../../api/admin-api.service.generated';

@Injectable({ providedIn: 'root' })
export class WorkForceOverviewService {
  constructor(
    private employeeApi: EmployeeApiClient,
    private departmentApi: DepartmentApiClient,
    private leaveApi: LeaveRequestApiClient,
    private salaryApi: SalaryApiClient,
    private attendanceApi: AttendanceApiClient,
  ) {}

  getDashboard() {
    return forkJoin({
      employees: this.employeeApi.getEmployees().pipe(catchError(() => of([]))),
      departments: this.departmentApi
        .getDepartments()
        .pipe(catchError(() => of([]))),
      leaves: this.leaveApi.getLeaveRequests().pipe(catchError(() => of([]))),
      salaries: this.salaryApi.getSalaries().pipe(catchError(() => of([]))),
      attendance: this.attendanceApi
        .getAttendances()
        .pipe(catchError(() => of([]))),
    }).pipe(
      map((result: any) => {
        const now = new Date();
        const curMonth = now.getMonth();
        const curYear = now.getFullYear();
        const todayStr = now.toDateString();

        // 1. Tính tổng lương thực nhận trong tháng này
        const totalPayroll = (result.salaries || []).reduce(
          (sum: number, s: any) => {
            const d = new Date(s.ngayTinhLuong);
            if (d.getMonth() === curMonth && d.getFullYear() === curYear) {
              return sum + (s.luongThucNhan || 0);
            }
            return sum;
          },
          0,
        );

        // 2. Chuyên cần hôm nay
        const todayAttendance = (result.attendance || []).filter(
          (a: any) =>
            a.checkInTime &&
            new Date(a.checkInTime).toDateString() === todayStr,
        ).length;

        // 3. Đơn nghỉ trong tháng
        const monthLeaves = (result.leaves || []).filter((l: any) => {
          const d = new Date(l.startDate || l.ngayXinNghi);
          return d.getMonth() === curMonth && d.getFullYear() === curYear;
        }).length;

        const activeEmps = (result.employees || []).filter(
          (e: any) => e.isActive,
        );

        return {
          stats: [
            {
              title: 'Nhân Viên',
              value: activeEmps.length,
              icon: 'cilPeople',
              color: 'primary',
              suffix: 'người',
            },
            {
              title: 'Phòng Ban',
              value: (result.departments || []).length,
              icon: 'cilHome',
              color: 'info',
              suffix: 'đơn vị',
            },
            {
              title: `Đơn Nghỉ (T${curMonth + 1})`,
              value: monthLeaves,
              icon: 'cilTask',
              color: 'warning',
              suffix: 'đơn',
            },
            {
              title: `Quỹ Lương (T${curMonth + 1})`,
              value: totalPayroll,
              icon: 'cilMoney',
              color: 'danger',
              suffix: 'VNĐ',
            },
          ],
          attendanceRate:
            activeEmps.length > 0
              ? Math.round((todayAttendance / activeEmps.length) * 100)
              : 0,
          todayAttendance,
          totalActive: activeEmps.length,
          recentEmployees: (result.employees || [])
            .sort(
              (a: any, b: any) =>
                new Date(b.hireDate).getTime() - new Date(a.hireDate).getTime(),
            )
            .slice(0, 5),
        };
      }),
    );
  }
}
