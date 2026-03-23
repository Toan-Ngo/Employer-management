using System.Collections.Generic;

namespace HRMS.Core.RBAC
{
    public static class RolePermissions
    {
        public static readonly Dictionary<string, List<string>> RolesMap = new()
        {
            [Roles.Admin] = new List<string>
            {
                Permissions.Account.View,
                Permissions.Account.Create,
                Permissions.Account.Update,
                Permissions.Account.Delete,

                Permissions.Employee.View,
                Permissions.Employee.Create,
                Permissions.Employee.Update,
                Permissions.Employee.Delete,

                Permissions.Department.View,
                Permissions.Department.Create,
                Permissions.Department.Update,
                Permissions.Department.Delete,

                Permissions.Position.View,
                Permissions.Position.Create,
                Permissions.Position.Update,
                Permissions.Position.Delete,

                Permissions.Contract.View,
                Permissions.Contract.Create,
                Permissions.Contract.Update,
                Permissions.Contract.Delete,

                Permissions.Attendance.View,
                Permissions.Attendance.Manage,
                Permissions.Attendance.Update,
                Permissions.Attendance.CheckIn,
                Permissions.Attendance.CheckOut,

                Permissions.LeaveRequest.View,
                Permissions.LeaveRequest.Create,
                Permissions.LeaveRequest.Update,
                Permissions.LeaveRequest.Approve,
                Permissions.LeaveRequest.Reject,

                Permissions.Salary.View,
                Permissions.Salary.Update,
                Permissions.Salary.Manage,

                Permissions.Report.View, 
            },

            [Roles.HR] = new List<string>
            {
                Permissions.Account.View,
                Permissions.Employee.View,
                Permissions.Employee.Create,
                Permissions.Employee.Update,
                Permissions.Department.View,
                Permissions.Position.View,
                Permissions.Contract.View,
                Permissions.Attendance.View,
                Permissions.LeaveRequest.View,
                Permissions.LeaveRequest.Approve,
                Permissions.LeaveRequest.Reject,
                Permissions.Report.View
            },

            [Roles.Employee] = new List<string>
            {
    
                Permissions.Attendance.View,
                Permissions.Attendance.CheckIn,
                Permissions.Attendance.CheckOut,
                Permissions.LeaveRequest.View,
                Permissions.LeaveRequest.Create,
                Permissions.LeaveRequest.Update,
                Permissions.Salary.View,
                Permissions.Report.View  
            }
        };
    }
}