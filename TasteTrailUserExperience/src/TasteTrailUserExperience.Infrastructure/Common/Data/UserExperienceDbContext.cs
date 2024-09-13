using Microsoft.EntityFrameworkCore;
using TasteTrailUserExperience.Core.FeedbackLikes.Models;
using TasteTrailUserExperience.Core.Feedbacks.Models;
using TasteTrailUserExperience.Core.MenuItemLikes.Models;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Core.Menus.Models;
using TasteTrailUserExperience.Core.Users.Models;
using TasteTrailUserExperience.Core.Venues.Models;
using TasteTrailUserExperience.Infrastructure.FeedbackLikes.Configurations;
using TasteTrailUserExperience.Infrastructure.Feedbacks.Configurations;
using TasteTrailUserExperience.Infrastructure.MenuItemLikes.Configurations;
using TasteTrailUserExperience.Infrastructure.MenuItems.Configurations;
using TasteTrailUserExperience.Infrastructure.Menus.Configurations;
using TasteTrailUserExperience.Infrastructure.Venues.Configurations;

namespace TasteTrailUserExperience.Infrastructure.Common.Data;

public class UserExperienceDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Venue> Venues { get; set; }

    public DbSet<Menu> Menus { get; set; }

    public DbSet<MenuItem> MenuItems { get; set; }

    public DbSet<Feedback> Feedbacks { get; set; }

    public DbSet<FeedbackLike> FeedbackLikes { get; set; }

    public DbSet<MenuItemLike> MenuItemLikes { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new VenueConfiguration());
        modelBuilder.ApplyConfiguration(new MenuConfiguration());
        modelBuilder.ApplyConfiguration(new MenuItemConfigurations());
        modelBuilder.ApplyConfiguration(new FeedbackConfiguration());
        modelBuilder.ApplyConfiguration(new FeedbackLikeConfiguration());
        modelBuilder.ApplyConfiguration(new MenuItemLikesConfiguration());
    }
}
