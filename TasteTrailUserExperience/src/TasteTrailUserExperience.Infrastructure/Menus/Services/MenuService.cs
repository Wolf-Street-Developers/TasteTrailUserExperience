using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Menus.Models;
using TasteTrailUserExperience.Core.Menus.Repositories;
using TasteTrailUserExperience.Core.Menus.Services;

namespace TasteTrailUserExperience.Infrastructure.Menus.Services;

public class MenuService : IMenuService
{
    private readonly IMenuRepository _menuRepository;

    public MenuService(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    public async Task<FilterResponseDto<Menu>> GetMenusFilteredAsync(PaginationParametersDto paginationParameters, int venueId)
    {
        if (venueId <= 0)
            throw new ArgumentException($"Invalid Venue ID: {venueId}.");

        var newFilterParameters = new FilterParameters<Menu>() {
            PageNumber = paginationParameters.PageNumber,
            PageSize = paginationParameters.PageSize,
            Specification = null,
            SearchTerm = null
        };

        var menus = await _menuRepository.GetFilteredByIdAsync(newFilterParameters, venueId);

        var totalMenus = await _menuRepository.GetCountFilteredIdAsync(newFilterParameters, venueId);
        var totalPages = (int)Math.Ceiling(totalMenus / (double)paginationParameters.PageSize);


        var filterReponse = new FilterResponseDto<Menu>() {
            CurrentPage = paginationParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalMenus,
            Entities = menus
        };

        return filterReponse;
    }

    public async Task<FilterResponseDto<Menu>> GetMenusFilteredAsync(PaginationSearchParametersDto paginationSearchParameters)
    {
        var newFilterParameters = new FilterParameters<Menu>() {
            PageNumber = paginationSearchParameters.PageNumber,
            PageSize = paginationSearchParameters.PageSize,
            Specification = null,
            SearchTerm = paginationSearchParameters.SearchTerm
        };

        var menus = await _menuRepository.GetFilteredAsync(newFilterParameters);

        var totalMenus = await _menuRepository.GetCountFilteredAsync(newFilterParameters);
        var totalPages = (int)Math.Ceiling(totalMenus / (double)paginationSearchParameters.PageSize);


        var filterReponse = new FilterResponseDto<Menu>() {
            CurrentPage = paginationSearchParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalMenus,
            Entities = menus
        };

        return filterReponse;
    }

    public async Task<Menu?> GetMenuByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var menu = await _menuRepository.GetByIdAsync(id);

        return menu;
    }
}
