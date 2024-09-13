#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TasteTrailUserExperience.Core.Common.Models;
using TasteTrailUserExperience.Core.Feedbacks.Models;
using TasteTrailUserExperience.Core.Menus.Models;

namespace TasteTrailUserExperience.Core.Venues.Models;

public class Venue : ICreateable, IRateable, IFeedbackable
{
    public int Id { get; set; }

    public required string Name { get; set; }
    
    public required string Address { get; set; }

    public double Longtitude { get; set; }
    
    public double Latitude { get; set; }

    public string? Description { get; set; }

    public string? ContactNumber { get; set; }

    public required string Email { get; set; }

    public string? LogoUrlPath { get; set; }

    public float AveragePrice { get; set; }

    [Range(0, 5)]
    public decimal Rating { get; set; }
    
    public DateTime CreationDate { get; set; }

    [JsonIgnore]
    public ICollection<Menu> Menus { get; set; }

    [JsonIgnore]
    public ICollection<Feedback> Feedbacks { get; set; }

    public required string UserId { get; set; }
}
