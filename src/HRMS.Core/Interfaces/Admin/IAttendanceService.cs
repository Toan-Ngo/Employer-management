using HRMS.Core.DTOs;

public interface IAttendanceService
{
    Task<List<AttendanceDto>> GetAttendances();

    public Task<int> GetTodayAttendanceAsync();

    Task<List<AttendanceDto>> GetAttendancesByEmployeeId(string employeeCode);

    Task<bool> CheckIn(int employeeId);

    Task<bool> CheckOut(int attendanceId);

    Task<bool> UpdateAttendance(UpdateAttendanceDto dto);

    Task<bool> DeleteAttendance(int attendanceId);
}