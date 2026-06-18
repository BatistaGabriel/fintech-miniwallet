using Fintech.MiniWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintech.MiniWallet.Infrastructure.Persistence;

public class MiniWalletDbContext : DbContext
{
    public MiniWalletDbContext(DbContextOptions<MiniWalletDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountHolder> Holders => Set<AccountHolder>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(builder =>
        {
            builder.HasKey(a => a.Id);

            builder.HasOne(a => a.Holder)
                    .WithMany()
                    .HasForeignKey("HolderId");

            builder.OwnsOne(a => a.Balance, b =>
            {
                b.Property(p => p.Amount)
                    .HasColumnName("Balance")
                    .HasPrecision(18, 2);
            });

            builder.HasMany(a => a.Transactions)
                .WithOne(t => t.OriginAccount)
                .HasForeignKey("OriginAccountId");
        });

        modelBuilder.Entity<Transaction>(builder =>
        {
            builder.HasKey(a => a.Id);
            builder.OwnsOne(a => a.Amount, b =>
            {
                b.Property(p => p.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2);
            });
        });

        base.OnModelCreating(modelBuilder);
    }
}