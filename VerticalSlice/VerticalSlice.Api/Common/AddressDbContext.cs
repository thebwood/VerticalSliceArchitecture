using Microsoft.EntityFrameworkCore;
using VerticalSlice.Api.Domain;

namespace VerticalSlice.Api.Common;

public class AddressDbContext : DbContext
{
    public AddressDbContext(DbContextOptions<AddressDbContext> options) : base(options)
    {
    }

    public DbSet<Address> Addresses => Set<Address>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Street).IsRequired().HasMaxLength(200);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.State).IsRequired().HasMaxLength(50);
            entity.Property(e => e.ZipCode).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt);
        });
    }
}
