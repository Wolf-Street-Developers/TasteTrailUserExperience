using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Menus.Models;

namespace TasteTrailUserExperience.Core.Menus.Services;

public interface IMenuService
{
    Task<FilterResponseDto<Menu>> GetMenusFilteredAsync(PaginationParametersDto filterParameters, int venueId);

    Task<FilterResponseDto<Menu>> GetMenusFilteredAsync(PaginationSearchParametersDto filterParameters);

    Task<Menu?> GetMenuByIdAsync(int id);
}
