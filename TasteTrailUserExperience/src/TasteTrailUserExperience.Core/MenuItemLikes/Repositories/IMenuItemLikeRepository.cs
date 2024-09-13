using TasteTrailData.Core.Common.Repositories.Base;
using TasteTrailUserExperience.Core.MenuItemLikes.Models;

namespace TasteTrailUserExperience.Core.MenuItemLikes.Repositories;

public interface IMenuItemLikeRepository : ICreateAsync<MenuItemLike, int>, IDeleteByIdAsync<int, int?>, IGetAsNoTrackingAsync<MenuItemLike, int>
{
    Task<List<int>> GetLikedMenuItemIds(string userId);

    Task<bool> Exists(int menuItemId, string userId);
}
