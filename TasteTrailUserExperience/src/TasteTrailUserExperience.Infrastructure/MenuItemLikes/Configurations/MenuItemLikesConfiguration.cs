using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailUserExperience.Core.MenuItemLikes.Models;

namespace TasteTrailUserExperience.Infrastructure.MenuItemLikes.Configurations;

public class MenuItemLikesConfiguration : IEntityTypeConfiguration<MenuItemLike>
{
    public void Configure(EntityTypeBuilder<MenuItemLike> builder)
    {
        builder.HasKey(mi => mi.Id);

        builder.Property(u => u.MenuItemId)
            .IsRequired();

        builder.Property(u => u.UserId)
            .IsRequired();

        builder
            .HasIndex(mi => new {mi.MenuItemId , mi.UserId})
            .IsUnique();
    }
}
