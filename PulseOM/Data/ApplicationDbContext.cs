using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace PulseOM.Data
{
    [SuppressMessage("ReSharper", "UnassignedField.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<PulseDataItem> PulseData { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PulseDataItem>()
                .HasKey(d => d.PulseDataItemId);

            modelBuilder.Entity<IdentityUser>()
                .HasMany<PulseDataItem>()
                .WithOne()
                .HasForeignKey(p => p.IdentityUserId)
                .IsRequired(false)
                .HasPrincipalKey(i => i.Id);
        }
    }
}