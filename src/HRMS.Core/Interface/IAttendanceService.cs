using HRMS.Core.Domain.Entities;

namespace HRMS.Core.Interface
{
    public interface IAttendanceService
    {
        Task<List<Attendance>> GetAttendances();
        Task<List<Attendance>> GetAttendancesByEmployeeId(int employeeId);
        Task<bool> CheckIn(int employeeId);
        Task<bool> CheckOut(int attendanceId);
        Task<bool> UpdateAttendance(Attendance attendance);
        Task<bool> DeleteAttendance(int attendanceId);
    }
}