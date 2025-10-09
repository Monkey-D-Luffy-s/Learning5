using Learning5.Models.Account;
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

        public DbSet<Roles> UserRoles { get; set; }

        public DbSet<TimeSheet> TimeSheets { get; set; }
        public DbSet<Designations> Designations { get; set; }
        public DbSet<Colleges> Colleges { get; set; }
        public DbSet<LeavesModule> LeavesModules { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Explicitly configure the unique index on the NormalizedUserName for your Employee entity
            builder.Entity<User>()
                .HasIndex(u => u.UserName)
                .IsUnique();
        }
    }
}
