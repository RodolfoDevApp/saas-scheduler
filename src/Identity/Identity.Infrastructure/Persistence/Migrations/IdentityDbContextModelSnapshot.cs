using System;
using Identity.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Identity.Infrastructure.Persistence.Migrations;

[DbContext(typeof(IdentityDbContext))]
partial class IdentityDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.0-preview.4.24267.4")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        modelBuilder.Entity("Identity.Domain.Entities.Role", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uniqueidentifier");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            b.HasKey("Id");

            b.HasIndex("Name")
                .IsUnique();

            b.ToTable("Roles", (string)null);
        });

        modelBuilder.Entity("Identity.Domain.Entities.Tenant", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uniqueidentifier");

            b.Property<bool>("Active")
                .HasColumnType("bit");

            b.Property<DateTime>("CreatedAt")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.Property<string>("Slug")
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnType("nvarchar(150)");

            b.HasKey("Id");

            b.HasIndex("Slug")
                .IsUnique();

            b.ToTable("Tenants", (string)null);
        });

        modelBuilder.Entity("Identity.Domain.Entities.User", b =>
        {
            b.Property<Guid>("Id")
                .HasColumnType("uniqueidentifier");

            b.Property<DateTime>("CreatedAt")
                .ValueGeneratedOnAdd()
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            b.Property<string>("Email")
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar(256)");

            b.Property<bool>("IsActive")
                .HasColumnType("bit");

            b.Property<string>("Name")
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            b.Property<string>("PasswordHash")
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("nvarchar(512)");

            b.Property<string>("Phone")
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            b.Property<Guid>("TenantId")
                .HasColumnType("uniqueidentifier");

            b.HasKey("Id");

            b.HasIndex("TenantId");

            b.HasIndex("TenantId", "Email")
                .IsUnique();

            b.ToTable("Users", (string)null);
        });

        modelBuilder.Entity("Identity.Domain.Entities.UserRole", b =>
        {
            b.Property<Guid>("UserId")
                .HasColumnType("uniqueidentifier");

            b.Property<Guid>("RoleId")
                .HasColumnType("uniqueidentifier");

            b.HasKey("UserId", "RoleId");

            b.HasIndex("RoleId");

            b.ToTable("UserRoles", (string)null);
        });

        modelBuilder.Entity("Identity.Domain.Entities.UserRole", b =>
        {
            b.HasOne("Identity.Domain.Entities.Role", "Role")
                .WithMany("Users")
                .HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.HasOne("Identity.Domain.Entities.User", "User")
                .WithMany("Roles")
                .HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Role");

            b.Navigation("User");
        });

        modelBuilder.Entity("Identity.Domain.Entities.User", b =>
        {
            b.HasOne("Identity.Domain.Entities.Tenant", "Tenant")
                .WithMany("Users")
                .HasForeignKey("TenantId")
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();

            b.Navigation("Tenant");
        });

        modelBuilder.Entity("Identity.Domain.Entities.Role", b =>
        {
            b.Navigation("Users");
        });

        modelBuilder.Entity("Identity.Domain.Entities.Tenant", b =>
        {
            b.Navigation("Users");
        });

        modelBuilder.Entity("Identity.Domain.Entities.User", b =>
        {
            b.Navigation("Roles");
        });
#pragma warning restore 612, 618
    }
}
