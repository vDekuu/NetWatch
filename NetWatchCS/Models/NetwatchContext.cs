using Microsoft.EntityFrameworkCore;

namespace NetWatchCS.Models;

public partial class NetwatchContext : DbContext
{
    public NetwatchContext()
    {
    }

    public NetwatchContext(DbContextOptions<NetwatchContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActiveDomain> ActiveDomains { get; set; }

    public string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Domains","netwatch.db");

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite($"Data Source={dbPath}");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActiveDomain>(entity =>
        {
            entity.ToTable("activeDomains");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Domain)
                  .HasColumnName("domain");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
