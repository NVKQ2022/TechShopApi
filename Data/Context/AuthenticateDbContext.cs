
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using TechShop.API.Models;
using TechShop_API_backend_.Helpers;
using TechShop_API_backend_.Models.Authenticate;

namespace TechShop_API_backend_.Data.Context
{

    public class AuthenticateDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserId> UserId { get; set; }

        public DbSet<AuthProvider> AuthProviders { get; set; }

        public DbSet<VerificationCode> VerificationCodes { get; set; }

        public DbSet<UserFcm> UserFcm { get; set; }


        public AuthenticateDbContext(DbContextOptions<AuthenticateDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.Username).IsUnique();
            });

            modelBuilder.Entity<UserId>(entity =>
            {
                entity.ToTable("userId");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID").ValueGeneratedNever();
                entity.HasData(new UserId { Id = 10000 });
            });

            modelBuilder.Entity<UserFcm>(entity =>
            {
                entity.ToTable("USER_FCM");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.UserId).HasColumnName("USER_ID").ValueGeneratedNever();
                entity.Property(e => e.FcmToken).HasColumnName("FCM_TOKEN").HasMaxLength(255).IsRequired();
            });

            modelBuilder.Entity<VerificationCode>(entity =>
            {
                entity.ToTable("VERIFICATION_CODES");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("ID");
                entity.Property(e => e.UserId).HasColumnName("USER_ID");
                entity.Property(e => e.Email).HasColumnName("EMAIL").HasMaxLength(255).IsRequired();
                entity.Property(e => e.Code).HasColumnName("CODE").HasMaxLength(100).IsRequired();
                entity.Property(e => e.Type).HasColumnName("TYPE").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ExpiresAt).HasColumnName("EXPIRES_AT");
                entity.Property(e => e.IsUsed).HasColumnName("IS_USED");
                entity.Property(e => e.CreatedAt).HasColumnName("CREATED_AT");
                entity.Property(e => e.UsedAt).HasColumnName("USED_AT");
            });

            modelBuilder.Entity<AuthProvider>(entity =>
            {
                entity.ToTable("auth_providers");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.UserId).HasColumnName("user_id");
                entity.Property(e => e.Provider).HasColumnName("provider").HasMaxLength(50).IsRequired();
                entity.Property(e => e.ProviderUserId).HasColumnName("provider_user_id").HasMaxLength(255).IsRequired();
                entity.Property(e => e.ProviderEmail).HasColumnName("provider_email").HasMaxLength(255);
                entity.Property(e => e.AccessToken).HasColumnName("access_token");
                entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
                entity.Property(e => e.TokenExpiresAt).HasColumnName("token_expires_at");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }

}