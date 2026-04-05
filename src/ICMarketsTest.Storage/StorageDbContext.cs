using ICMarketsTest.Storage.BlockchainSnapshots;
using Microsoft.EntityFrameworkCore;

namespace ICMarketsTest.Storage;

public sealed class StorageDbContext(DbContextOptions<StorageDbContext> options) : DbContext(options)
{
    public DbSet<BlockchainSnapshot> BlockchainSnapshots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BlockchainSnapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.NetworkId).HasMaxLength(32).IsRequired();
            entity.Property(e => e.Payload).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => new { e.NetworkId }); // Add index for querying snapshots history
        });
    }
}