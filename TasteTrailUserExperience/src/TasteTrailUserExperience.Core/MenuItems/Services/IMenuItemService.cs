using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.MenuItems.Dtos;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Core.MenuItems.Services;

public interface IMenuItemService
{
    Task<FilterResponseDto<MenuItemGetDto>> GetMenuItemsFilteredAsync(FilterParametersSearchDto filterParameters, int menuId, User? user);

    Task<FilterResponseDto<MenuItemGetDto>> GetMenuItemsFilteredAsync(FilterParametersSearchDto filterParameters, User? user);

    Task<MenuItem?> GetMenuItemByIdAsync(int id);
}
