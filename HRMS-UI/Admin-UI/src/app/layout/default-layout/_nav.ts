import { INavData } from '@coreui/angular';

export const navItems: INavData[] = [
  {
    name: 'Tổng Quan',
    url: '/workforce-overview',
    iconComponent: { name: 'cilSpeedometer' }
  },

  {
    title: true,
    name: 'EMPLOYEE MANAGEMENT'
  },

  {
    name: 'Nhân Sự',
    url: '/employees',
    iconComponent: { name: 'cilPeople' }
  },

  {
    name: 'Phòng Ban',
    url: '/department',
    iconComponent: { name: 'cilLayers' }
  },

  {
    name: 'Chức Vụ',
    url: '/positions',
    iconComponent: { name: 'cilUser' }
  },

  {
    name: 'Hợp Đồng',
    url: '/contracts',
    iconComponent: { name: 'cilDescription' }
  },

  {
    title: true,
    name: 'WORK MANAGEMENT'
  },

  {
    name: 'Chấm Công',
    url: '/attendance',
    iconComponent: { name: 'cilTask' }
  },

  {
    name: 'Đơn Nghỉ',
    url: '/leave-request',
    iconComponent: { name: 'cilCalendar' }
  },

  {
    title: true,
    name: 'FINANCE'
  },

  {
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
];