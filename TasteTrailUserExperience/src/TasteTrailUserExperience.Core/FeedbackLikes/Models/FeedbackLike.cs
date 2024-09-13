namespace TasteTrailUserExperience.Core.FeedbackLikes.Models;

public class FeedbackLike
{
    public int Id { get; set; }
    
    public required int FeedbackId { get; set; }

    public required string UserId { get; set; }
}
