using HRMS.Core.Interfaces.Admin;

namespace HRMS.Data.Services.Admin
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HRMSContext _context;
        public UnitOfWork(HRMSContext context)
        {
            _context = context;
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
