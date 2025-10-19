using Microsoft.EntityFrameworkCore;

using MVMedia.Api.Models;
using MVMedia.API.Models;

namespace MVMedia.Api.Context;

public class ApiDbContext : DbContext
{
    public ApiDbContext(DbContextOptions<ApiDbContext> opt) : base(opt){}

    public DbSet<Client> Clients { get; set; }
    public DbSet<Media> Medias { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<MediaFile> MediaFiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Relacionamento 1x1 de MediaFile para Media
        modelBuilder.Entity<Media>()
            .HasOne(m => m.MediaFile)
            .WithOne(mf => mf.Media)
            .HasForeignKey<Media>(m => m.MediaFileId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MediaFile>()
            .HasOne(mf => mf.Media)
            .WithOne(m => m.MediaFile)
            .HasForeignKey<MediaFile>(mf => mf.MediaId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
