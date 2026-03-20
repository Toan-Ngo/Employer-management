// workforce-overview.service.ts
import { Injectable } from '@angular/core';
import { forkJoin, of, map, catchError, tap } from 'rxjs';

import {
  EmployeeApiClient,
  DepartmentApiClient,
  LeaveRequestApiClient,
  SalaryApiClient,
  AttendanceApiClient
} from '../../api/admin-api.service.generated';

@Injectable({
  providedIn: 'root'
})
export class WorkForceOverviewService {

  constructor(
    private employeeApi: EmployeeApiClient,
    private departmentApi: DepartmentApiClient,
    private leaveApi: LeaveRequestApiClient,
    private salaryApi: SalaryApiClient,
    private attendanceApi: AttendanceApiClient
  ) { }

  getDashboard() {
    return forkJoin({
      employees: this.employeeApi.getEmployees().pipe(catchError(err => {
        console.error('Employees API error:', err);
        return of([]);
      })),
      departments: this.departmentApi.getDepartments().pipe(catchError(err => {
        console.error('Departments API error:', err);
        return of([]);
      })),
      leaves: this.leaveApi.getLeaveRequests().pipe(catchError(err => {
        console.error('Leaves API error:', err);
        return of([]);
      })),
      salaries: this.salaryApi.getSalaries().pipe(catchError(err => {
        console.error('Salaries API error:', err);
        return of([]);
      })),
      attendance: this.attendanceApi.getAttendances().pipe(catchError(err => {
        console.error('Attendance API error:', err);
        return of([]);
      })),
    }).pipe(
      // 
      tap(result => console.log('Raw dashboard result:', result)),
      map((result: any) => {
        const employeesData = result.employees || [];
        const departmentsData = result.departments || [];
        const leavesData = result.leaves || [];
        const salariesData = result.salaries || [];
        const attendanceData = result.attendance || [];

        const activeEmployees = employeesData.filter((emp: any) => emp.isActive === true);
        const totalEmployees = activeEmployees.length;
        const totalDepartments = departmentsData.length;
        const totalLeaves = leavesData.length;
        const totalPayroll = salariesData.reduce((sum: number, s: any) => sum + (s?.amount || 0), 0);

        // Tính attendance hôm nay
        const today = new Date().toISOString().split('T')[0];
        const todayAttendance = attendanceData.filter((a: any) => a?.date?.startsWith(today)).length;
        const attendanceRate = totalEmployees > 0 ? Math.round((todayAttendance / totalEmployees) * 100) : 0;

        // Map nhân viên về format component cần
        const employees = employeesData
          .sort((a: any, b: any) => new Date(b.hireDate || 0).getTime() - new Date(a.hireDate || 0).getTime())
          .slice(0, 5)
          // 
          .map((emp: any) => ({
            employeeCode: emp.employeeCode,
            fullName: emp.fullName || `${emp.firstName || ''} ${emp.lastName || ''}`.trim(),
            departmentName: emp.departmentName || emp.department?.departmentName || '',
            positionName: emp.positionName || emp.position?.positionName || '',
            hireDate: emp.hireDate || new Date().toISOString(),
            isActive: emp.isActive !== undefined ? emp.isActive : true
          }));

        const dashboardData = {
          totalEmployees,
          totalDepartments,
          totalLeaves,
          totalPayroll,
          todayAttendance,
          attendanceRate,
          employees
        };

        console.log('Formatted Dashboard data:', dashboardData);

        return dashboardData;
      })
    );
  }
}