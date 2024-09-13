#pragma warning disable CS8618

using System.Text.Json.Serialization;
using TasteTrailUserExperience.Core.MenuItems.Models;

namespace TasteTrailUserExperience.Core.Menus.Models;

public class Menu
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? ImageUrlPath { get; set; }
    
    public int VenueId { get; set; }

    public string UserId { get; set; }

    [JsonIgnore]
    public ICollection<MenuItem> MenuItems { get; set; }
}
