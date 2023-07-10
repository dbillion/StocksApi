using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StocksApi.Models;

public partial class StockDbContext : DbContext
{
    public StockDbContext()
    {
    }

    public StockDbContext(DbContextOptions<StockDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<StockMaster> StockMasters { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=StockDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockMaster>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockMas__3214EC07EA44F54D");

            entity.ToTable("StockMaster");

            entity.Property(e => e.BuyPrice).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Qty).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.StockId).HasMaxLength(50);
            entity.Property(e => e.StockName).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
