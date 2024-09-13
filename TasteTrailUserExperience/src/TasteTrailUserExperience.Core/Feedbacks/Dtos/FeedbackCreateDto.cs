namespace TasteTrailUserExperience.Core.Feedbacks.Dtos;

public class FeedbackCreateDto
{
    public string? Text { get; set; }

    public int Rating { get; set; }
    
    public int VenueId { get; set; }
}
