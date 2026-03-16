using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using Microsoft.EntityFrameworkCore;
using static HRMS.Core.Entities.LeaveRequest;


namespace HRMS.Data.Services.Admin
{
    public class LeaveRequestService : ILeaveRequestService
    {
        private readonly HRMSContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public LeaveRequestService(HRMSContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }
        public async Task<List<LeaveRequest>> GetLeaveRequests()
        {
            return await _context.LeaveRequests.ToListAsync();
        }
        public async Task<List<LeaveRequest>> GetLeaveRequestByEmployee(string employeeId)
        {
            var code = employeeId.Trim().ToLower();

            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.Employee.EmployeeCode.ToLower() == code)
                .ToListAsync();
        }

        public async Task<bool> CreateLeaveRequest(LeaveRequest request)
        {
            await _context.LeaveRequests.AddAsync(request);
            
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FirstOrDefaultAsync(e => e.Id == id);
            if (leaveRequest == null)
                return false;
            _context.LeaveRequests.Remove(leaveRequest);
            return await _unitOfWork.SaveChangesAsync() > 0;
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
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> ApproveLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
                return false;

            request.Status = LeaveStatus.Approved;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> RejectLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);

            if (request == null)
                return false;

            request.Status = LeaveStatus.Rejected;

            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}
