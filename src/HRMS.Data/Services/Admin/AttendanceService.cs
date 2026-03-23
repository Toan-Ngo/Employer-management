using HRMS.Core.Entities;
using HRMS.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using HRMS.Core.Interfaces.Admin;

namespace HRMS.Data.Services.Admin
{
    public class AttendanceService : IAttendanceService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public AttendanceService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> GetTodayAttendanceAsync()
        {
            var today = DateTime.Today;

            return await _context.Attendances
                .Where(a => a.AttendanceDate.Date == today && a.CheckInTime != null)
                .CountAsync();
        }

        public async Task<List<AttendanceDto>> GetAttendances()
        {
            return await _context.Attendances
               
                .Select(a => new AttendanceDto
                {
                    Id = a.Id,
                    EmployeeCode = a.Employee.EmployeeCode,
                    FullName = a.Employee.FirstName + a.Employee.LastName, 
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime
                })
                .OrderByDescending(a => a.CheckInTime) 
                .ToListAsync();
        }

        public async Task<List<AttendanceDto>> GetAttendancesByEmployeeId(string employeeCode)
        {
            employeeCode = employeeCode.ToLower().Trim();

            return await _context.Attendances
                .Where(a => a.Employee.EmployeeCode.ToLower() == employeeCode)
                .Select(a => new AttendanceDto
                {
                    Id = a.Id,
                    EmployeeCode = a.Employee.EmployeeCode,
                    FullName = a.Employee.FirstName + a.Employee.LastName,
                    CheckInTime = a.CheckInTime,
                    CheckOutTime = a.CheckOutTime
                })
                .OrderByDescending(a => a.CheckInTime)
                .ToListAsync();
        }

        public async Task<bool> UpdateAttendance(UpdateAttendanceDto dto)
        {
            var attendance = await _context.Attendances
                .FirstOrDefaultAsync(a => a.Id == dto.Id);

            if (attendance == null)
                return false;

            attendance.CheckInTime = dto.CheckInTime;
            attendance.CheckOutTime = dto.CheckOutTime;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAttendance(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);

            if (attendance == null)
                return false;

            _context.Attendances.Remove(attendance);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckIn(int employeeId)
        {
            var today = DateTime.Today;

            var existed = await _context.Attendances
                .AnyAsync(a => a.EmployeeId == employeeId &&
                               a.CheckInTime.HasValue &&
                               a.CheckInTime.Value.Date == today);

            if (existed)
                return false;

            var attendance = new Attendance
            {
                EmployeeId = employeeId,
                CheckInTime = DateTime.Now
            };

            await _context.Attendances.AddAsync(attendance);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> CheckOut(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);

            if (attendance == null || attendance.CheckOutTime != null)
                return false;

            attendance.CheckOutTime = DateTime.Now;

            _context.Attendances.Update(attendance);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}