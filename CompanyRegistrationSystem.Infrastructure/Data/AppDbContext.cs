using CompanyRegistrationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

// Infrastructure/AppDbContext.cs
using Microsoft.EntityFrameworkCore;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<OtpCode> OtpCodes { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<OtpCode>()
            .HasOne(o => o.Company)
            .WithMany()
            .HasForeignKey(o => o.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}