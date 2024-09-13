namespace TasteTrailUserExperience.Core.MenuItemLikes.Models;

public class MenuItemLike
{
    public int Id { get; set; }
    
    public required int MenuItemId { get; set; }

    public required string UserId { get; set; }
}
