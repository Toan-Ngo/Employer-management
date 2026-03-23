namespace HRMS.Core.RBAC
{
    public static class Permissions
    {
        public static class Account
        {
            public const string View = "Permissions.Account.View";
            public const string Create = "Permissions.Account.Create";
            public const string Update = "Permissions.Account.Update";
            public const string Delete = "Permissions.Account.Delete";
        }

        public static class Employee
        {
            public const string View = "Permissions.Employee.View";
            public const string Create = "Permissions.Employee.Create";
            public const string Update = "Permissions.Employee.Update";
            public const string Delete = "Permissions.Employee.Delete";
        }

        public static class Department
        {
            public const string View = "Permissions.Department.View";
            public const string Create = "Permissions.Department.Create";
            public const string Update = "Permissions.Department.Update";
            public const string Delete = "Permissions.Department.Delete";
        }

        public static class Position
        {
            public const string View = "Permissions.Position.View";
            public const string Create = "Permissions.Position.Create";
            public const string Update = "Permissions.Position.Update";
            public const string Delete = "Permissions.Position.Delete";
        }

        public static class Attendance
        {
            public const string View = "Permissions.Attendance.View";
            public const string Update = "Permissions.Attendance.Update";
            public const string Delete = "Permissions.Attendance.Delete";
            public const string CheckIn = "Permissions.Attendance.CheckIn";
            public const string CheckOut = "Permissions.Attendance.CheckOut";
            public const string Manage = "Permissions.Attendance.Manage";
        }

        public static class Contract
        {
            public const string View = "Permissions.Contract.View";
            public const string Create = "Permissions.Contract.Create";
            public const string Update = "Permissions.Contract.Update";
            public const string Delete = "Permissions.Contract.Delete";
        }

        public static class Salary
        {
            public const string View = "Permissions.Salary.View";
            public const string Create = "Permissions.Salary.Create";
            public const string Update = "Permissions.Salary.Update";
            public const string Delete = "Permissions.Salary.Delete";
            public const string Manage = "Permissions.Salary.Manage";
        }

        public static class LeaveRequest
        {
            public const string View = "Permissions.Leave.View";
            public const string Create = "Permissions.Leave.Create";
            public const string Update = "Permissions.Leave.Update";
            public const string Delete = "Permissions.Leave.Delete";
            public const string Approve = "Permissions.Leave.Approve";
            public const string Reject = "Permissions.Leave.Reject";
        }

        public static class Report
        {
            public const string View = "Permissions.Report.View";
            public const string Manage = "Permissions.Report.Manage";
        }
    }
}