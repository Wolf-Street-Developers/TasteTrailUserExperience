using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailUserExperience.Core.MenuItems.Models;

namespace TasteTrailUserExperience.Infrastructure.MenuItems.Configurations;

public class MenuItemConfigurations : IEntityTypeConfiguration<MenuItem>
{
    public void Configure(EntityTypeBuilder<MenuItem> builder)
    {
        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(mi => mi.Description)
            .HasMaxLength(500);

        builder.Property(mi => mi.Price)
            .IsRequired();

        builder.Property(mi => mi.Likes)
            .IsRequired();

        builder.Property(mi => mi.MenuId)
            .IsRequired();

        builder.Property(mi => mi.UserId)
            .IsRequired();

        builder.HasMany(u => u.MenuItemLikes)
            .WithOne()
            .HasForeignKey(r => r.MenuItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
