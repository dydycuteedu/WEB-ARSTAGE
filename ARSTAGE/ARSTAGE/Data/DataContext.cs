using ARSTAGE.Models;

namespace ARSTAGE.Data
{
    public class DataContext : IdentityDbContext<AppUserModel>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<AppUserModel> Users { get; set; }
        public DbSet<ArtistConfirmationModel> ArtistConfirmations { get; set; }
        public DbSet<ArtistModel> Artists { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CommentModel> Comments { get; set; }
        public DbSet<FavouriteModel> Favorites { get; set; }
        public DbSet<FollowModel> Follows { get; set; }
        public DbSet<LikeModel> Likes { get; set; }
        public DbSet<MessageFileModel> MessageFiles { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<PostCategoryModel> PostCategories { get; set; }
        public DbSet<PostModel> Posts { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ReviewModel> Reviews { get; set; }
        public DbSet<TransactionModel> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<FavouriteModel>()
                .HasKey(f => new { f.UserID, f.PostID })
                .IsClustered(false);
            builder.Entity<FavouriteModel>()
                .HasOne(f => f.User)
                .WithMany() // hoặc WithMany(u => u.Favourites) nếu bạn có navigation ngược
                .HasForeignKey(f => f.UserID);
            builder.Entity<FavouriteModel>()
                .HasOne(f => f.Post)
                .WithMany() // hoặc WithMany(p => p.Favourites)
                .HasForeignKey(f => f.PostID);
            builder.Entity<FavouriteModel>()
                .HasOne(f => f.User)
                .WithMany(u => u.Favorites)
                .HasForeignKey(f => f.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<FavouriteModel>()
                .HasOne(f => f.Post)
                .WithMany(p => p.Favorites)
                .HasForeignKey(f => f.PostID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<FollowModel>()
                .HasKey(f => new { f.FollowerID, f.FollowingID });
            builder.Entity<FollowModel>()
                .HasOne(f => f.Follower)
                .WithMany()
                .HasForeignKey(f => f.FollowerID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<FollowModel>()
                .HasOne(f => f.Following)
                .WithMany()
                .HasForeignKey(f => f.FollowingID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<LikeModel>()
                .HasKey(l => new { l.UserID, l.PostID })
                .IsClustered(false);
            builder.Entity<LikeModel>()
                .HasOne(l => l.User)
                .WithMany()
                .HasForeignKey(l => l.UserID)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<LikeModel>()
                .HasOne(l => l.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.PostID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MessageFileModel>()
                .HasOne(mf => mf.Message)
                .WithMany(m => m.Files)
                .HasForeignKey(mf => mf.MessageID);

            builder.Entity<PostCategoryModel>()
                .HasKey(pc => new { pc.PostID, pc.CategoryID });
            builder.Entity<PostCategoryModel>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.PostCategories)
                .HasForeignKey(pc => pc.PostID);
            builder.Entity<PostCategoryModel>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.PostCategories)
                .HasForeignKey(pc => pc.CategoryID);

            builder.Entity<MessageModel>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<MessageModel>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<CommissionModel>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<CommissionModel>()
                .HasOne(c => c.Artist)
                .WithMany()
                .HasForeignKey(c => c.ArtistID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<ReviewModel>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserID)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<ReviewModel>()
                .HasOne(r => r.Artist)
                .WithMany(a => a.Reviews)
                .HasForeignKey(r => r.ArtistID)
                .OnDelete(DeleteBehavior.NoAction);
            SeedRoles(builder);
        }
        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData
            (
                new IdentityRole
                {
                    Id = "1",  // Role ID cho Admin
                    Name = "Admin",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = "2",  // Role ID cho Member
                    Name = "Member",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    NormalizedName = "MEMBER"
                },
                new IdentityRole
                {
                    Id = "3",  // Role ID cho Artist
                    Name = "Artist",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    NormalizedName = "ARTIST"
                }
            );
        }
    }
}
