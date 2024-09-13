#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TasteTrailUserExperience.Core.Common.Models;
using TasteTrailUserExperience.Core.FeedbackLikes.Models;

namespace TasteTrailUserExperience.Core.Feedbacks.Models;

public class Feedback : ICreateable, IRateable, ILikeable
{
    public int Id { get; set; }

    public string? Text { get; set; }

    [Range(0, 5)]
    public decimal Rating { get; set; }

    public DateTime CreationDate { get; set; }

    public int Likes { get; set; }

    public required string UserId { get; set; }
    
    public int VenueId { get; set; }

    [JsonIgnore]
    public ICollection<FeedbackLike> FeedbackLikes { get; set; }
}
