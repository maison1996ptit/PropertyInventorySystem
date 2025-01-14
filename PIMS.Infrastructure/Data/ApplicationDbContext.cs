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
        public DbSet<PropertyPriceAudit> PropertyPriceAudits { get; set; }
        public DbSet<PriceOfAcquisition> PriceOfAcquisitions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PriceOfAcquisition>()
                .HasKey(pc => new { pc.PropertyId, pc.ContactId });

            modelBuilder.Entity<PriceOfAcquisition>()
                .HasOne(pc => pc.Property)
                .WithMany(p => p.PropertyContacts)
                .HasForeignKey(pc => pc.PropertyId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<PriceOfAcquisition>()
                .HasOne(pc => pc.Contact)
                .WithMany(c => c.PropertyContacts)
                .HasForeignKey(pc => pc.ContactId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PropertyPriceAudit>()
               .HasOne(pa => pa.Property)  
               .WithMany(p => p.propertyPriceAudits) 
               .HasForeignKey(pa => pa.PropertyId)  
               .OnDelete(DeleteBehavior.Cascade);  
        }
    }   
}
