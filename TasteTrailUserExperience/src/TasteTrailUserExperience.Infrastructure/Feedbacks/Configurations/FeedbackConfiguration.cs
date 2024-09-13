using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TasteTrailUserExperience.Core.Feedbacks.Models;

namespace TasteTrailUserExperience.Infrastructure.Feedbacks.Configurations;

public class FeedbackConfiguration : IEntityTypeConfiguration<Feedback>
{
    public void Configure(EntityTypeBuilder<Feedback> builder)
    {
        builder.HasKey(f => f.Id); 

        builder.Property(f => f.Text)
            .IsRequired();

        builder.Property(f => f.Rating)
            .IsRequired()
            .HasPrecision(7, 3);

        builder.Property(f => f.Likes)
            .IsRequired();

        builder.Property(f => f.CreationDate)
            .IsRequired();

        builder.HasMany(u => u.FeedbackLikes)
            .WithOne()
            .HasForeignKey(r => r.FeedbackId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
