using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Loffers.Server.Models
{
    public partial class LoffersContext : DbContext
    {
        public LoffersContext()
        {
        }

        public LoffersContext(DbContextOptions<LoffersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Categories> Categories { get; set; }
        public virtual DbSet<Locations> Locations { get; set; }
        public virtual DbSet<OfferCategories> OfferCategories { get; set; }
        public virtual DbSet<OfferComplaints> OfferComplaints { get; set; }
        public virtual DbSet<OfferLocations> OfferLocations { get; set; }
        public virtual DbSet<Offers> Offers { get; set; }
        public virtual DbSet<PublisherLocationCategories> PublisherLocationCategories { get; set; }
        public virtual DbSet<PublisherLocations> PublisherLocations { get; set; }
        public virtual DbSet<Publishers> Publishers { get; set; }
        public virtual DbSet<UserCategories> UserCategories { get; set; }
        public virtual DbSet<UserStarredOffers> UserStarredOffers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Data Source=IDCSQL3.znetlive.com,1234;Initial Catalog=acrm_livedb_loffersData;user=acrm_livedb_loffers;password=Admin@123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>(entity =>
            {
                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Locations>(entity =>
            {
                entity.HasKey(e => e.LocationId);

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DisplayAddress)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.Lat).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.Long).HasColumnType("decimal(18, 9)");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<OfferCategories>(entity =>
            {
                entity.HasKey(e => e.OfferCategoryId);

                entity.Property(e => e.OfferCategoryId).HasColumnName("OfferCategoryID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedBy)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.OfferCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferCategories_Categories");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.OfferCategories)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferCategories_Offers");
            });

            modelBuilder.Entity<OfferComplaints>(entity =>
            {
                entity.HasKey(e => e.ReportId);

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ReportContent)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.OfferComplaints)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferComplaints_Offers");
            });

            modelBuilder.Entity<OfferLocations>(entity =>
            {
                entity.HasKey(e => e.OfferLocationId);

                entity.Property(e => e.OfferLocationId).HasColumnName("OfferLocationID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.PublisherLocationId).HasColumnName("PublisherLocationID");

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTill).HasColumnType("datetime");

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.OfferLocations)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OfferLocations_Offers");

                entity.HasOne(d => d.PublisherLocation)
                    .WithMany(p => p.OfferLocations)
                    .HasForeignKey(d => d.PublisherLocationId)
                    .HasConstraintName("FK_OfferLocations_PublisherLocations");
            });

            modelBuilder.Entity<Offers>(entity =>
            {
                entity.HasKey(e => e.OfferId);

                entity.Property(e => e.OfferId).HasColumnName("OfferID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.OfferDescription)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.OfferHeadline)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.TermsAndConditions)
                    .IsRequired()
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.ValidFrom).HasColumnType("datetime");

                entity.Property(e => e.ValidTill).HasColumnType("datetime");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Offers)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Offers_Publishers");
            });

            modelBuilder.Entity<PublisherLocationCategories>(entity =>
            {
                entity.HasKey(e => e.PublisherLocationCategoryId);

                entity.Property(e => e.PublisherLocationCategoryId).HasColumnName("PublisherLocationCategoryID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.PublisherLocationId).HasColumnName("PublisherLocationID");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.PublisherLocationCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_PublisherLocationCategories_Categories");

                entity.HasOne(d => d.PublisherLocation)
                    .WithMany(p => p.PublisherLocationCategories)
                    .HasForeignKey(d => d.PublisherLocationId)
                    .HasConstraintName("FK_PublisherLocationCategories_PublisherLocations");
            });

            modelBuilder.Entity<PublisherLocations>(entity =>
            {
                entity.HasKey(e => e.PublisherLocationId);

                entity.Property(e => e.PublisherLocationId).HasColumnName("PublisherLocationID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.PublisherLocations)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PublisherLocations_Locations");

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.PublisherLocations)
                    .HasForeignKey(d => d.PublisherId)
                    .HasConstraintName("FK_PublisherLocations_Publishers");
            });

            modelBuilder.Entity<Publishers>(entity =>
            {
                entity.HasKey(e => e.PublisherId);

                entity.Property(e => e.PublisherId).HasColumnName("PublisherID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasMaxLength(300)
                    .IsUnicode(false);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .HasMaxLength(70)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedBy)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserCategories>(entity =>
            {
                entity.HasKey(e => e.UserCategoryId)
                    .HasName("PK_UserCateories");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.UserCategories)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCateories_Categories");
            });

            modelBuilder.Entity<UserStarredOffers>(entity =>
            {
                entity.HasKey(e => e.StarredOfferId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LastEditedOn).HasColumnType("datetime");

                entity.Property(e => e.OfferLocationId).HasColumnName("OfferLocationID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Offer)
                    .WithMany(p => p.UserStarredOffers)
                    .HasForeignKey(d => d.OfferId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserStarredOffers_Offers");

                entity.HasOne(d => d.OfferLocation)
                    .WithMany(p => p.UserStarredOffers)
                    .HasForeignKey(d => d.OfferLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserStarredOffers_UserStarredOffers");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
