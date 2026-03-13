namespace HRMS.Core.Interface
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
