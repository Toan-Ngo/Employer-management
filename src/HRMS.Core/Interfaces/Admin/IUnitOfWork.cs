namespace HRMS.Core.Interfaces.Admin
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
