using ContactService.Models;
using Microsoft.EntityFrameworkCore;

namespace ContactService.DAL
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext()
        {

        }

        public ContactDbContext(DbContextOptions<ContactDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<ContactInfo> ContactInfos { get; set; }
        public virtual DbSet<ContactInfoType> ContactInfoTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ContactInfos");

                entity.Property(e => e.Value)
                      .IsRequired();

                entity.HasOne(d => d.Person)
                      .WithMany(p => p.ContactInfos)
                      .HasForeignKey(d => d.PersonId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_ContactInfos_People");

                entity.HasOne(d => d.ContactInfoType)
                      .WithMany(p => p.ContactInfos)
                      .HasForeignKey(d => d.ContactInfoTypeId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_ContactInfos_ContactInfoTypes");
            });

            modelBuilder.Entity<ContactInfoType>(entity =>
            {
                entity.ToTable("ContactInfoTypes");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(150);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("People");

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Surname)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(e => e.Company)
                      .HasMaxLength(250);
            });
        }
    }
}
