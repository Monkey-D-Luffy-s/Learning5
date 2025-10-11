using Learning5.Models.Account;
using Learning5.Models.PaySlabs;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Learning5.data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }

        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<Roles> UserRoles { get; set; }

        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Designations> Designations { get; set; }
        public DbSet<Colleges> Colleges { get; set; }
        public DbSet<LeavesModule> LeavesModules { get; set; }

        public DbSet<BankDetails> BankDetails { get; set; }

        public DbSet<StaturaryDetailsEmployee> EmployeeStaturary { get; set; }

        public DbSet<EmployeeSalary> EmployeeSalaries { get; set; }

        public DbSet<EmployeeTaxDeclarations> EmployeeTaxDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}
