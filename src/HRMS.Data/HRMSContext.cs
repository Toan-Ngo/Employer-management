using HRMS.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Data
{
    public class HRMSContext : IdentityDbContext<ApplicationUser>
    {
        public HRMSContext(DbContextOptions<HRMSContext> options) : base(options)
        {
        }
        // Định nghĩa DbSet cho các thực thể
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        // Cấu hình quan hệ giữa các thực thể
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Cấu hình quan hệ giữa Employee và Department
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);
            // Cấu hình quan hệ giữa Employee và Position
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.Position)
                .WithMany(p => p.Employees)
                .HasForeignKey(e => e.PositionId);
            // Cấu hình quan hệ giữa Employee và các thực thể khác
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Attendances)
                .HasForeignKey(a => a.EmployeeId);
            // Cấu hình quan hệ giữa Employee và Contract
            modelBuilder.Entity<Contract>()
                .HasOne(c => c.Employee)
                .WithMany(e => e.Contracts)
                .HasForeignKey(c => c.EmployeeId);
            // Cấu hình quan hệ giữa Employee và Salary
            modelBuilder.Entity<Salary>()
                .HasOne(s => s.Employee)
                .WithMany(e => e.Salaries)
                .HasForeignKey(s => s.EmployeeId);
            //  Cấu hình quan hệ giữa Employee và LeaveRequest
            modelBuilder.Entity<LeaveRequest>()
                .HasOne(l => l.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(l => l.EmployeeId);
            // Cấu hình quan hệ giữa ApplicationUser và Employee
            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Employee)
                .WithOne()
                .HasForeignKey<ApplicationUser>(u => u.EmployeeId);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is Employee && (e.State == EntityState.Added || e.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                var employee = (Employee)entry.Entity;
                if (entry.State == EntityState.Added)
                {
                    employee.CreatedAt = DateTime.UtcNow;
                    employee.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    employee.UpdatedAt = DateTime.UtcNow;
                }
            }
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
