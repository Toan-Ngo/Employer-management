// _nav.ts
import { INavData } from '@coreui/angular';
import { PERMISSIONS } from '../../shared/constants/permissions.constants';

export const navItems = (permissions: string[]): INavData[] => {
  const has = (p: string) => permissions.includes(p);

  return [
    {
      name: 'Tổng Quan',
      url: '/workforce-overview',
      iconComponent: { name: 'cilSpeedometer' }
    },

    {
      title: true,
      name: 'EMPLOYEE MANAGEMENT'
    },

    has(PERMISSIONS.EMPLOYEE.VIEW) && {
      name: 'Nhân Sự',
      url: '/employees',
      iconComponent: { name: 'cilPeople' }
    },

    has(PERMISSIONS.DEPARTMENT.VIEW) && {
      name: 'Phòng Ban',
      url: '/department',
      iconComponent: { name: 'cilLayers' }
    },

    has(PERMISSIONS.POSITION.VIEW) && {
      name: 'Chức Vụ',
      url: '/positions',
      iconComponent: { name: 'cilUser' }
    },

    has(PERMISSIONS.CONTRACT.VIEW) && {
      name: 'Hợp Đồng',
      url: '/contracts',
      iconComponent: { name: 'cilDescription' }
    },

    {
      title: true,
      name: 'WORK MANAGEMENT'
    },

    has(PERMISSIONS.ATTENDANCE.VIEW) && {
      name: 'Chấm Công',
      url: '/attendance',
      iconComponent: { name: 'cilTask' }
    },

    has(PERMISSIONS.LEAVE_REQUEST.VIEW) && {
      name: 'Đơn Nghỉ',
      url: '/leave-request',
      iconComponent: { name: 'cilCalendar' }
    },

    {
      title: true,
      name: 'FINANCE'
    },

    has(PERMISSIONS.SALARY.VIEW) && {
      name: 'Lương',
      url: '/payroll',
      iconComponent: { name: 'cilDollar' }
    },

    {
      name: 'Phản Hồi',
      url: '/reports',
      iconComponent: { name: 'cilChart' }
    },

    {
      title: true,
      name: 'SYSTEM'
    },

    {
      name: 'Cài Đặt',
      url: '/settings',
      iconComponent: { name: 'cilSettings' }
    }
  ].filter(Boolean) as INavData[];
};