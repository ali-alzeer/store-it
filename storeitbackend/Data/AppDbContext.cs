
#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using storeitbackend.Dtos.Account;
using storeitbackend.Dtos.File;
using storeitbackend.Models;

namespace storeitbackend.Data
{
  public class AppDbContext : IdentityDbContext<User>, IAppDbContext
  {
    public DbSet<File> Files { get; set; }
    public DbSet<FileType> FileTypes { get; set; }
    public DbSet<FileExtension> FileExtensions { get; set; }
    public DbSet<UserFile> UsersFiles { get; set; }
    public DbSet<OwnerFile> OwnersFiles { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
          => base.SaveChangesAsync(cancellationToken);
    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // ************ DEFAULT SCHEMA ************
      builder.HasDefaultSchema("dbo");

      // ************ ROLES ************
      builder.Entity<IdentityRole>(o =>
      {
        o.Property<string>("Id").HasColumnType("varchar(36)").HasMaxLength(36).HasDefaultValueSql("UPPER(CAST(NEWID() AS VARCHAR(36)))").ValueGeneratedOnAdd();
        o.Property<string>("Name").HasMaxLength(50).IsRequired(true);
        o.Property<string>("NormalizedName").HasMaxLength(50).IsRequired(true);
        o.ToTable(name: "Roles", schema: "identity");
      });

      // ************ USERS ************
      builder.Entity<User>(o =>
      {
        // Configuring data types
        o.Property<string>("Id").HasColumnType("varchar(36)").HasMaxLength(36).HasDefaultValueSql("UPPER(CAST(NEWID() AS VARCHAR(36)))").ValueGeneratedOnAdd();
        o.Property<string>("FirstName").HasMaxLength(15).IsRequired(true);
        o.Property<string>("LastName").HasMaxLength(15).IsRequired(true);
        o.Property<DateTime>("CreatedAt").IsRequired(true);
        o.Property<string>("Email").HasMaxLength(50).IsRequired(true);
        o.Property<string>("NormalizedEmail").HasMaxLength(50).IsRequired(true);
        o.Property<string>("UserName").HasMaxLength(50).IsRequired(true);
        o.Property<string>("NormalizedUserName").HasMaxLength(50).IsRequired(true);
        o.Property<string>("PasswordHash").HasMaxLength(256).IsRequired(true);
        o.Property<string>("ImageUrl").HasMaxLength(256);
        // Ignoring columns
        o.Ignore("EmailConfirmed").Ignore("PhoneNumber").Ignore("PhoneNumberConfirmed").Ignore("LockoutEnd").Ignore("LockoutEnabled").Ignore("AccessFailedCount");

        o.ToTable(name: "Users", schema: "identity");

      });

      // ************ ROLES_CLAIMS ************
      builder.Entity<IdentityRoleClaim<string>>(o =>
      {
        o.Property<string>("ClaimType").HasMaxLength(50).IsRequired(true);
        o.Property<string>("ClaimValue").HasMaxLength(50).IsRequired(true);
        o.ToTable(name: "RolesClaims", schema: "identity");
      });

      // ************ USER_CLAIMS ************
      builder.Entity<IdentityUserClaim<string>>(o =>
      {
        o.Property<string>("ClaimType").HasMaxLength(50).IsRequired(true);
        o.Property<string>("ClaimValue").HasMaxLength(50).IsRequired(true);
        o.ToTable(name: "UsersClaims", schema: "identity");
      });

      // ************ USER_LOGINS ************
      builder.Entity<IdentityUserLogin<string>>(o =>
      {
        o.HasKey(l => new { l.LoginProvider, l.ProviderKey });
        o.Property("ProviderDisplayName").HasMaxLength(100).IsRequired(true);
        o.ToTable(name: "UsersLogins", schema: "identity");
      });

      // ************ USER_ROLES ************
      builder.Entity<IdentityUserRole<string>>(o =>
      {
        o.HasKey(l => new { l.UserId, l.RoleId });
        o.ToTable(name: "UsersRoles", schema: "identity");
      });

      // ************ USER_TOKENS ************
      builder.Entity<IdentityUserToken<string>>(o =>
      {
        o.HasKey(l => new { l.UserId, l.LoginProvider, l.Name });
        o.Property("Value").HasMaxLength(256).IsRequired(true);
        o.ToTable(name: "UsersTokens", schema: "identity");
      });

      // **************************************
      // **************************************

      // ************ FILES ************
      builder.Entity<File>(o =>
      {
        o.Property<string>("Id").HasColumnType("varchar(36)").HasMaxLength(36).HasDefaultValueSql("UPPER(CAST(NEWID() AS VARCHAR(36)))").ValueGeneratedOnAdd();
        o.HasOne(f => f.Type).WithMany().HasForeignKey(f => f.TypeId);
        o.HasOne(f => f.Extension).WithMany().HasForeignKey(f => f.ExtensionId);
        o.ToTable(name: "Files", schema: "files");
      });

      builder.Entity<UserFile>()
        .HasKey(uf => new { uf.UserId, uf.FileId }); // Composite Key

      builder.Entity<UserFile>()
          .HasOne(uf => uf.User)
          .WithMany(u => u.UsersFiles)
          .HasForeignKey(uf => uf.UserId);

      builder.Entity<UserFile>()
          .HasOne(uf => uf.File)
          .WithMany(f => f.UsersFiles)
          .HasForeignKey(uf => uf.FileId);

      builder.Entity<UserFile>().ToTable("UsersFiles", schema: "dbo");

      builder.Entity<FileType>().ToTable("FileTypes", schema: "files").Property<string>("Id").HasColumnType("varchar(36)").HasMaxLength(36).HasDefaultValueSql("UPPER(CAST(NEWID() AS VARCHAR(36)))").ValueGeneratedOnAdd();
      builder.Entity<FileExtension>().ToTable("FileExtensions", schema: "files").Property<string>("Id").HasColumnType("varchar(36)").HasMaxLength(36).HasDefaultValueSql("UPPER(CAST(NEWID() AS VARCHAR(36)))").ValueGeneratedOnAdd(); ;


      builder.Entity<OwnerFile>()
        .HasKey(of => new { of.UserId, of.FileId }); // Composite Key

      builder.Entity<OwnerFile>()
          .HasOne(of => of.User)
          .WithMany(o => o.OwnersFiles)
          .HasForeignKey(of => of.UserId);

      builder.Entity<OwnerFile>()
          .HasOne(of => of.File)
          .WithMany(f => f.OwnersFiles)
          .HasForeignKey(of => of.FileId);

      builder.Entity<OwnerFile>().ToTable("OwnersFiles", schema: "dbo");

      builder.Entity<UserFileDto>().HasNoKey();
      builder.Entity<UserFileSharedDto>().HasNoKey();
    }
  }
}