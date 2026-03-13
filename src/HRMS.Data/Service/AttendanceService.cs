using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;
namespace HRMS.Data.Service
{
    public class AttendanceService : IAttendanceService
    {
        private readonly HRMSContext _context;
        public AttendanceService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<List<Attendance>> GetAttendances()
        {
            return await _context.Attendances.ToListAsync();
        }
        public async Task<List<Attendance>> GetAttendancesByEmployeeId(string employeeId)
        {
            return await _context.Attendances.Include(a => a.Employee)
                .Where(a => a.Employee.EmployeeCode.ToLower().Trim() == employeeId.ToLower().Trim())
                .ToListAsync();
        }

        public async Task<bool> UpdateAttendance(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> DeleteAttendance(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance == null)
                return false;

            _context.Attendances.Remove(attendance);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<bool> CheckIn(int Id)
        {
            var today = DateTime.Today;

            var existed = await _context.Attendances
                .AnyAsync(a => a.EmployeeId == Id &&
                               a.CheckInTime.HasValue &&
                               a.CheckInTime.Value.Date == today);

            if (existed)
                return false;
            var attendance = new Attendance
            {
                EmployeeId = Id,
                CheckInTime = DateTime.Now
            };
            await _context.Attendances.AddAsync(attendance);
            return await _context.SaveChangesAsync() > 0;);
        }

        public async Task<bool> CheckOut(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance == null || attendance.CheckOutTime != null)
                return await Task.FromResult(false);
            attendance.CheckOutTime = DateTime.Now;
            _context.Attendances.Update(attendance);
            return await _context.SaveChangesAsync() > 0;
        }
}
