using TasteTrailData.Core.Filters.Specifications;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.MenuItemLikes.Repositories;
using TasteTrailUserExperience.Core.MenuItems.Dtos;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Core.MenuItems.Repositories;
using TasteTrailUserExperience.Core.MenuItems.Services;
using TasteTrailUserExperience.Core.Menus.Repositories;
using TasteTrailUserExperience.Core.Users.Models;
using TasteTrailUserExperience.Infrastructure.MenuItems.Factories;

namespace TasteTrailUserExperience.Infrastructure.MenuItems.Services;

public class MenuItemService : IMenuItemService
{
    private readonly IMenuItemRepository _menuItemRepository;

    private readonly IMenuItemLikeRepository _menuItemLikeRepository;

    public MenuItemService(IMenuItemRepository menuItemRepository, IMenuRepository menuRepository, IMenuItemLikeRepository menuItemLikeRepository)
    {
        _menuItemRepository = menuItemRepository;
        _menuItemLikeRepository = menuItemLikeRepository;
    }

    public async Task<FilterResponseDto<MenuItemGetDto>> GetMenuItemsFilteredAsync(FilterParametersSearchDto filterParameters, int menuId, User? user)
    {
        if (menuId <= 0)
            throw new ArgumentException($"Invalid Venue ID: {menuId}.");

        var newFilterParameters = new FilterParameters<MenuItem>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = MenuItemFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = filterParameters.SearchTerm
        };

        var menuItems = await _menuItemRepository.GetFilteredByIdAsync(newFilterParameters, menuId);
        var menuItemDtos = new List<MenuItemGetDto>();

        List<int>? likedMenuItemIds = null;

        if (user is not null)
            likedMenuItemIds = await _menuItemLikeRepository.GetLikedMenuItemIds(user.Id);

        foreach (var menuItem in menuItems)
        {
            var isLiked = likedMenuItemIds is not null && likedMenuItemIds.Any(id => id == menuItem.Id);

            var menuItemDto = new MenuItemGetDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                ImageUrlPath = menuItem.ImageUrlPath,
                Price = menuItem.Price!,
                Likes = menuItem.Likes,
                MenuId = menuItem.MenuId,
                UserId = menuItem.UserId,
                IsLiked = isLiked
            };

            menuItemDtos.Add(menuItemDto);
        }

        var totalMenuItems = await _menuItemRepository.GetCountFilteredIdAsync(newFilterParameters, menuId);
        var totalPages = (int)Math.Ceiling(totalMenuItems / (double)filterParameters.PageSize);


        var filterReponse = new FilterResponseDto<MenuItemGetDto>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalMenuItems,
            Entities = menuItemDtos
        };

        return filterReponse;
    }

    
    public async Task<FilterResponseDto<MenuItemGetDto>> GetMenuItemsFilteredAsync(FilterParametersSearchDto filterParameters, User? user)
    {
        var newFilterParameters = new FilterParameters<MenuItem>() {
            PageNumber = filterParameters.PageNumber,
            PageSize = filterParameters.PageSize,
            Specification = MenuItemFilterFactory.CreateFilter(filterParameters.Type),
            SearchTerm = filterParameters.SearchTerm
        };

        var menuItems = await _menuItemRepository.GetFilteredAsync(newFilterParameters);
        var menuItemDtos = new List<MenuItemGetDto>();

        List<int>? likedMenuItemIds = null;

        if (user is not null)
            likedMenuItemIds = await _menuItemLikeRepository.GetLikedMenuItemIds(user.Id);

        foreach (var menuItem in menuItems)
        {
            var isLiked = likedMenuItemIds is not null && likedMenuItemIds.Any(id => id == menuItem.Id);

            var menuItemDto = new MenuItemGetDto
            {
                Id = menuItem.Id,
                Name = menuItem.Name,
                Description = menuItem.Description,
                ImageUrlPath = menuItem.ImageUrlPath,
                Price = menuItem.Price!,
                Likes = menuItem.Likes,
                MenuId = menuItem.MenuId,
                UserId = menuItem.UserId,
                IsLiked = isLiked
            };

            menuItemDtos.Add(menuItemDto);
        }

        var totalMenuItems = await _menuItemRepository.GetCountFilteredAsync(newFilterParameters);
        var totalPages = (int)Math.Ceiling(totalMenuItems / (double)filterParameters.PageSize);


        var filterReponse = new FilterResponseDto<MenuItemGetDto>() {
            CurrentPage = filterParameters.PageNumber,
            AmountOfPages = totalPages,
            AmountOfEntities = totalMenuItems,
            Entities = menuItemDtos
        };

        return filterReponse;
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException($"Invalid ID value: {id}.");

        var menuItem = await _menuItemRepository.GetByIdAsync(id);

        return menuItem;
    }

    public async Task<int?> PutMenuItemAsync(MenuItemUpdateDto menuItem, User user)
    {
        var menuItemToUpdate = await _menuItemRepository.GetAsNoTrackingAsync(menuItem.Id);

        if (menuItemToUpdate is null)
            return null;

        if (menuItemToUpdate.UserId != user.Id)
            throw new ForbiddenAccessException();

        var updatedMenuItem = new MenuItem() {
            Id = menuItem.Id,
            Name = menuItem.Name,
            Description = menuItem.Description,
            Price = menuItem.Price,
            UserId = user.Id,
            MenuId = menuItemToUpdate.MenuId
        };

        var menuItemId = await _menuItemRepository.PutAsync(updatedMenuItem);

        return menuItemId;
    }
}
