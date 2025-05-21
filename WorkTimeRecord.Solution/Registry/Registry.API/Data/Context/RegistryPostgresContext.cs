using Microsoft.EntityFrameworkCore;
using Registry.API.Domain;

namespace Registry.API.Data.Context
{
    public class RegistryPostgresContext : DbContext, IRegistryContext
    {
        public RegistryPostgresContext(DbContextOptions<RegistryPostgresContext> options) : base(options) { }
        public DbSet<UserWorkTimeRecord> UserWorkTimeRecords { get; set; } 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserWorkTimeRecord>().ToTable("user_worktime_record");
            modelBuilder.Entity<UserWorkTimeRecord>().HasKey(x => x.UserName);

            modelBuilder.Entity<UserWorkTimeRecord>().Property(x => x.UserName).HasColumnName("user_name");
            modelBuilder.Entity<UserWorkTimeRecord>().Property(x => x.FirstName).HasColumnName("first_name");
            modelBuilder.Entity<UserWorkTimeRecord>().Property(x => x.LastName).HasColumnName("last_name");
            modelBuilder.Entity<UserWorkTimeRecord>().Property(x => x.LastRecord).HasColumnName("last_record")
                .HasConversion(
                    v => v.ToUniversalTime(), // Persistir en formato UTC
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc) // Leer como UTC
                    );
            modelBuilder.Entity<UserWorkTimeRecord>().Property(x => x.Mode).HasColumnName("mode");

        }
    }   
}
