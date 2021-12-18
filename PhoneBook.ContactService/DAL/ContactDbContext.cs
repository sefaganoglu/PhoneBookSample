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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ContactInfo>(entity =>
            {
                entity.ToTable("ContactInfos");


                entity.Property(e => e.Phone)
                       .HasMaxLength(25);

                entity.Property(e => e.Email)
                      .HasMaxLength(150); 

                entity.Property(e => e.Location)
                       .HasMaxLength(150);

                entity.HasOne(d => d.Person)
                      .WithMany(p => p.ContactInfos)
                      .HasForeignKey(d => d.PersonId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_ContactInfos_People");

                entity.HasIndex(p => p.Location);
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
