

using HRMS.Core.Domain.Entities;
using HRMS.Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data.Service
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly HRMSContext _context;
        public LeaveRequestService(HRMSContext context)
        {
            _context = context;
        }
        public async Task<List<LeaveRequest>> GetLeaveRequests()
        {
            return await _context.LeaveRequests.ToListAsync();
        }
        public async Task<List<LeaveRequest>> GetLeaveRequestByEmployee(string employeeId)
        {
            return await _context.LeaveRequests.Include(lr => lr.Employee).
                Where(e => e.Employee.EmployeeCode.ToLower().Trim() == employeeId.ToLower().Trim()).ToListAsync();
        }

        public async Task<bool> CreateLeaveRequest(LeaveRequest request)
        {
            await _context.LeaveRequests.AddAsync(request);
            
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(e => e.Id == id);
            if (leaveRequest == null)
                return false;
            _context.LeaveRequests.Remove(leaveRequest);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateLeaveRequest(LeaveRequest request)
        {
            var existing = await _context.LeaveRequests.FindAsync(request.Id);

            if (existing == null)
                return false;
            existing.StartDate = request.StartDate;
            existing.EndDate = request.EndDate;
            existing.Reason = request.Reason;
            existing.Status = request.Status;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
