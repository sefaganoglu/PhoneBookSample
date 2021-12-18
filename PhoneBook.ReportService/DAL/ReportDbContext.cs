using Microsoft.EntityFrameworkCore;
using PhoneBook.ReportService.Models;

namespace PhoneBook.ReportService.DAL
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext()
        {

        }

        public ReportDbContext(DbContextOptions<ReportDbContext> options) : base(options)
        {

        }

        public virtual DbSet<ReportInfo> ReportInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReportInfo>(entity =>
            {
                entity.ToTable("ReportInfos");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Status)
                      .HasMaxLength(50);
            });
        }
    }
}
