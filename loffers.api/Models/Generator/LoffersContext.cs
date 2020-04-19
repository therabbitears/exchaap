namespace loffers.api.Models.Generator
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using CodeFirstStoreFunctions;

    public partial class LoffersContext : DbContext
    {
        public LoffersContext()
            : base("name=DefaultConnection")
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
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<UserCategories> UserCategories { get; set; }
        public virtual DbSet<UserStarredOffers> UserStarredOffers { get; set; }
        public virtual DbSet<UseConfigurations> UseConfigurations { get; set; }
        public virtual DbSet<ChatGroups> ChatGroups { get; set; }
        public virtual DbSet<ChatGroupUsers> ChatGroupUsers { get; set; }
        public virtual DbSet<ChatGroupMessages> ChatGroupMessages { get; set; }
        public virtual DbSet<UserProfileSnapshots> UserProfileSnapshots { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categories>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.OfferCategories)
                .WithRequired(e => e.Categories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Categories>()
                .HasMany(e => e.UserCategories)
                .WithRequired(e => e.Categories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Locations>()
                .Property(e => e.Lat)
                .HasPrecision(18, 9);

            modelBuilder.Entity<Locations>()
                .Property(e => e.Long)
                .HasPrecision(18, 9);

            modelBuilder.Entity<Locations>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Locations>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Locations>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Locations>()
                .Property(e => e.DisplayAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Locations>()
                .HasMany(e => e.PublisherLocations)
                .WithRequired(e => e.Locations)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OfferCategories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OfferCategories>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OfferCategories>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<OfferComplaints>()
                .Property(e => e.ReportContent)
                .IsUnicode(false);

            modelBuilder.Entity<OfferComplaints>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OfferLocations>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OfferLocations>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<OfferLocations>()
                .HasMany(e => e.UserStarredOffers)
                .WithRequired(e => e.OfferLocations)
                .WillCascadeOnDelete(false);
            
            modelBuilder.Entity<Offers>()
                .Property(e => e.OfferHeadline)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .Property(e => e.OfferDescription)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .Property(e => e.TermsAndConditions)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Offers>()
                .HasMany(e => e.OfferCategories)
                .WithRequired(e => e.Offers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Offers>()
                .HasMany(e => e.OfferComplaints)
                .WithRequired(e => e.Offers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Offers>()
                .HasMany(e => e.OfferLocations)
                .WithRequired(e => e.Offers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Offers>()
                .HasMany(e => e.UserStarredOffers)
                .WithRequired(e => e.Offers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PublisherLocationCategories>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PublisherLocationCategories>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PublisherLocations>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PublisherLocations>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PublisherLocations>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<PublisherLocations>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .Property(e => e.Image)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .Property(e => e.LastEditedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<Publishers>()
                .HasMany(e => e.Offers)
                .WithRequired(e => e.Publishers)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserCategories>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<UserStarredOffers>()
                .Property(e => e.UserId)
                .IsUnicode(false);

            modelBuilder.Entity<UseConfigurations>()
              .Property(e => e.UserId)
              .IsUnicode(false);

            modelBuilder.Entity<ChatGroups>()
              .Property(e => e.CreatedBy)
              .IsUnicode(false);

            modelBuilder.Entity<ChatGroups>()
              .Property(e => e.Name)
              .IsUnicode(false);

            modelBuilder.Entity<ChatGroupUsers>()
             .Property(e => e.UserId)
             .IsUnicode(false);

            modelBuilder.Entity<ChatGroupMessages>()
              .Property(e => e.Message)
              .IsUnicode(true);

            modelBuilder.Conventions.Add(new FunctionsConvention("dbo", this.GetType()));

        }

        [DbFunction("CodeFirstDatabaseSchema", "DistanceCalculate")]
        public static decimal DistanceCalculate(decimal currentLat, decimal offerLat,
                                                           decimal currentLong, decimal offerLong)
        {
            // no need to provide an implementation
            throw new NotSupportedException();
        }
    }
}
