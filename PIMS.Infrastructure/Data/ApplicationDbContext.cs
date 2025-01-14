using Microsoft.EntityFrameworkCore;
using PIMS.Domain.Entities;

namespace PIMS.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Property> Properties { get; set; }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Property>(entity =>
            {
                entity.ToTable("Properties");
                entity.Property(e => e.Id)
                    .IsRequired();
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");
                entity.Property(e => e.Address)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");
                entity.Property(e => e.Price)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");
                entity.Property(e => e.OwnerId) // Foreign key referencing Contact.Id
                    .IsRequired(false); // OwnerId is optional
                entity.Property(e => e.CreatedDate)
                    .IsRequired()
                    .HasColumnType("datetime2");

                // Define the relationship and deletion behavior
                entity.HasOne<Contact>()
                    .WithMany(c => c.Properties)
                    .HasForeignKey(e => e.OwnerId) // Foreign key in Property referencing Contact.Id
                    .OnDelete(DeleteBehavior.SetNull); // Set OwnerId to NULL when Contact is deleted
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.ToTable("Contacts");
                entity.Property(e => e.Id)
                    .IsRequired();
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");
                entity.Property(e => e.LastName)
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");
                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnType("nvarchar");

                // Contact can have many Properties
                entity.HasMany(c => c.Properties)
                    .WithOne(p => p.Contact) // Property has one Contact
                    .HasForeignKey(p => p.OwnerId) // Foreign key in Property pointing to Contact.Id
                    .OnDelete(DeleteBehavior.SetNull); // Set OwnerId to NULL when Contact is deleted
            });
        }
    }   
}
