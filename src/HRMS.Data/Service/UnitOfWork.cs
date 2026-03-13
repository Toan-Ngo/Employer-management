using HRMS.Core.Interface;

namespace HRMS.Data.Service
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
