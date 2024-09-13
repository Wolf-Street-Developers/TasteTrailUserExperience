using TasteTrailUserExperience.Core.MenuItemLikes.Dtos;
using TasteTrailUserExperience.Core.Users.Models;


namespace TasteTrailUserExperience.Core.MenuItemLikes.Services;

public interface IMenuItemLikeService
{
    Task<int> CreateMenuItemLikeAsync(MenuItemLikeCreateDto menuItemLikeCreateDto, User user);

    Task<int?> DeleteMenuItemLikeByIdAsync(int id, User user);
}
