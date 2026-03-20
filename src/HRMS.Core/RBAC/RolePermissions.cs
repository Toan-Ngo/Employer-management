using System.Collections.Generic;

namespace HRMS.Core.RBAC
{
    public static class RolePermissions
    {
        public static readonly Dictionary<string, List<string>> RolesMap = new()
        {
            [Roles.Admin] = new List<string>
            {
                Permissions.Employee.View,
                Permissions.Employee.Create,
                Permissions.Employee.Update,
                Permissions.Employee.Delete,
                Permissions.Department.View,
                Permissions.Department.Create,
                Permissions.Department.Update,
                Permissions.Department.Delete,
                Permissions.Attendance.View,
                Permissions.Attendance.Update,
                Permissions.Attendance.CheckIn,
                Permissions.Attendance.CheckOut,
                Permissions.Salary.View,
                Permissions.Salary.Update,
                Permissions.LeaveRequest.Approve,
                Permissions.LeaveRequest.Reject
                // add tất cả quyền admin
            },

            [Roles.HR] = new List<string>
            {
                Permissions.Employee.View,
                Permissions.Employee.Create,
                Permissions.Employee.Update,
                Permissions.Department.View,
                Permissions.Attendance.View,
                Permissions.LeaveRequest.View,
                Permissions.LeaveRequest.Approve,
                Permissions.LeaveRequest.Reject
                // HR có subset quyền quản lý nhân sự, nghỉ phép
            },

            [Roles.Employee] = new List<string>
            {
                Permissions.Attendance.View,
                Permissions.Attendance.CheckIn,
                Permissions.Attendance.CheckOut,
                Permissions.LeaveRequest.View,
                Permissions.LeaveRequest.Create,
                Permissions.LeaveRequest.Update
                // Employee chỉ có quyền chấm công, nghỉ phép cá nhân
            }
        };
    }
}