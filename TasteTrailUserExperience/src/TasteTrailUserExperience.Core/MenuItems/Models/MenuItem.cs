#pragma warning disable CS8618

using System.Text.Json.Serialization;
using TasteTrailUserExperience.Core.Common.Models;
using TasteTrailUserExperience.Core.MenuItemLikes.Models;

namespace TasteTrailUserExperience.Core.MenuItems.Models;

public class MenuItem : ILikeable
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? ImageUrlPath { get; set; }

    public float Price { get; set; }

    public int Likes { get; set; }

    public required int MenuId { get; set; }

    public required string UserId { get; set; }
    
    [JsonIgnore]
    public ICollection<MenuItemLike> MenuItemLikes { get; set; }
}
