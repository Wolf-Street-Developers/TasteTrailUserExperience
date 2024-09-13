namespace TasteTrailUserExperience.Core.Feedbacks.Dtos;

public class FeedbackGetDto
{
    public int Id { get; set; }

    public string? Text { get; set; }

    public int Rating { get; set; }

    public required DateTime CreationDate { get; set; }

    public required string Username { get; set; }

    public required string UserId { get; set; }

    public required int VenueId { get; set; }

    public int Likes { get; set; }

    public bool IsLiked { get; set; }
}
