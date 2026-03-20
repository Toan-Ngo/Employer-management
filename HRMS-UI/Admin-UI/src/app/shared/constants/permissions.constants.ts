export const PERMISSIONS = {
    ACCOUNT: {
        VIEW: 'Permissions.Account.View',
        CREATE: 'Permissions.Account.Create',
        DELETE: 'Permissions.Account.Delete',
    },

    EMPLOYEE: {
        VIEW: 'Permissions.Employee.View',
        CREATE: 'Permissions.Employee.Create',
        UPDATE: 'Permissions.Employee.Update',
        DELETE: 'Permissions.Employee.Delete',
    },

    DEPARTMENT: {
        VIEW: 'Permissions.Department.View',
        CREATE: 'Permissions.Department.Create',
        UPDATE: 'Permissions.Department.Update',
        DELETE: 'Permissions.Department.Delete',
    },

    POSITION: {
        VIEW: 'Permissions.Position.View',
        CREATE: 'Permissions.Position.Create',
        UPDATE: 'Permissions.Position.Update',
        DELETE: 'Permissions.Position.Delete',
    },

    ATTENDANCE: {
        VIEW: 'Permissions.Attendance.View',
        UPDATE: 'Permissions.Attendance.Update',
        DELETE: 'Permissions.Attendance.Delete',
        CHECK_IN: 'Permissions.Attendance.CheckIn',
        CHECK_OUT: 'Permissions.Attendance.CheckOut',
    },

    CONTRACT: {
        VIEW: 'Permissions.Contract.View',
        CREATE: 'Permissions.Contract.Create',
        UPDATE: 'Permissions.Contract.Update',
        DELETE: 'Permissions.Contract.Delete',
    },

    SALARY: {
        VIEW: 'Permissions.Salary.View',
        CREATE: 'Permissions.Salary.Create',
        UPDATE: 'Permissions.Salary.Update',
        DELETE: 'Permissions.Salary.Delete',
    },

    LEAVE_REQUEST: {
        VIEW: 'Permissions.Leave.View',
        CREATE: 'Permissions.Leave.Create',
        UPDATE: 'Permissions.Leave.Update',
        DELETE: 'Permissions.Leave.Delete',
        APPROVE: 'Permissions.Leave.Approve',
        REJECT: 'Permissions.Leave.Reject',
    }
} as const;