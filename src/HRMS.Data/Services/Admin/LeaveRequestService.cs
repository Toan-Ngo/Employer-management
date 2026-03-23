using HRMS.Core.DTOs;
using HRMS.Core.Entities;
using HRMS.Core.Interfaces.Admin;
using HRMS.Data.SeedWorks; // Giả sử chứa IUnitOfWork
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
        public async Task<List<LeaveRequestDto>> GetLeaveRequests()
        {
            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new LeaveRequestDto
                {
                    Id = l.Id,
                    EmployeeCode = l.Employee.EmployeeCode,
                    EmployeeName = l.Employee.FirstName + " " + l.Employee.LastName,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    Status = l.Status.ToString(),
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<List<LeaveRequestDto>> GetLeaveRequestByEmployee(string employeeCode)
        {
            var code = employeeCode.Trim().ToLower();

            return await _context.LeaveRequests
                .Include(l => l.Employee)
                .Where(l => l.Employee.EmployeeCode.ToLower() == code)
                .OrderByDescending(l => l.CreatedAt)
                .Select(l => new LeaveRequestDto
                {
                    Id = l.Id,
                    EmployeeCode = l.Employee.EmployeeCode,
                    EmployeeName = l.Employee.FirstName + " " + l.Employee.LastName,
                    StartDate = l.StartDate,
                    EndDate = l.EndDate,
                    Reason = l.Reason,
                    Status = l.Status.ToString(),
                    CreatedAt = l.CreatedAt
                })
                .ToListAsync();
        }

        public async Task<bool> CreateLeaveRequest(CreateLeaveRequestDto request)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(x => x.EmployeeCode == request.EmployeeCode);

            if (employee == null) return false;

            var leave = new LeaveRequest
            {
                EmployeeId = employee.Id,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Reason = request.Reason,
                Status = LeaveStatus.Pending,
                CreatedAt = DateTime.Now
            };

            _context.LeaveRequests.Add(leave);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateLeaveRequest(int id, UpdateLeaveRequestDto request)
        {
            var leave = await _context.LeaveRequests.FindAsync(id);

            if (leave == null || leave.Status != LeaveStatus.Pending)
                return false;

            leave.StartDate = request.StartDate;
            leave.EndDate = request.EndDate;
            leave.Reason = request.Reason;

            _context.LeaveRequests.Update(leave);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteLeaveRequest(int id)
        {
            var leaveRequest = await _context.LeaveRequests.FindAsync(id);

            if (leaveRequest == null || leaveRequest.Status != LeaveStatus.Pending)
                return false;

            _context.LeaveRequests.Remove(leaveRequest);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> ApproveLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request == null) return false;

            request.Status = LeaveStatus.Approved;
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> RejectLeaveRequest(int id)
        {
            var request = await _context.LeaveRequests.FindAsync(id);
            if (request == null) return false;

            request.Status = LeaveStatus.Rejected;
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}