using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailData.Core.FeedbackLikes.Models;

namespace TasteTrailUserExperience.Infrastructure.FeedbackLikes.Configurations;

public class FeedbackLikeConfiguration : IEntityTypeConfiguration<FeedbackLike>
{
    public void Configure(EntityTypeBuilder<FeedbackLike> builder)
    {
        builder.HasKey(mi => mi.Id);

        builder.Property(u => u.FeedbackId)
            .IsRequired();

        builder.Property(u => u.UserId)
            .IsRequired();

        builder
            .HasIndex(mi => new {mi.FeedbackId , mi.UserId})
            .IsUnique();
    }
}
