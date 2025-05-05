using Microsoft.EntityFrameworkCore;

namespace OpenEcologyApp.Data
{
    public class EcologyDbContext : DbContext
    {
        public EcologyDbContext(DbContextOptions<EcologyDbContext> options)
            : base(options)
        {
        }

        public DbSet<GrainHarvest> GrainHarvests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ExportLog> ExportLogs { get; set; }
        public DbSet<ReportLog> ReportLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<GrainHarvest>()
                .HasKey(g => g.Id);

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.Region)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.Year)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.Yield)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.SownArea)
                .IsRequired();

            modelBuilder.Entity<GrainHarvest>()
                .Property(g => g.GrossHarvest)
                .IsRequired();

            // Конфигурация для таблицы Users
            modelBuilder.Entity<User>()
                .HasKey(u => u.UserId);

            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<User>()
                .Property(u => u.Status)
                .IsRequired()
                .HasDefaultValue(UserStatus.Active);

            modelBuilder.Entity<User>()
                .Property(u => u.DateRegistered)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Добавляем начального администратора
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    UserName = "admin",
                    Email = "admin@example.com",
                    Password = "admin123", // В реальном приложении пароль должен быть хеширован
                    IsAdmin = true,
                    Status = UserStatus.Active
                }
            );

            modelBuilder.Entity<ExportLog>()
                .Property(e => e.ExportedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<ReportLog>()
                .Property(r => r.ReportedAt)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
