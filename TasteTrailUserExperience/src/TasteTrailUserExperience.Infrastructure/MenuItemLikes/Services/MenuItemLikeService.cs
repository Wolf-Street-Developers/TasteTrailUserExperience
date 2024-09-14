using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.MenuItemLikes.Dtos;
using TasteTrailUserExperience.Core.MenuItemLikes.Models;
using TasteTrailUserExperience.Core.MenuItemLikes.Repositories;
using TasteTrailUserExperience.Core.MenuItemLikes.Services;
using TasteTrailUserExperience.Core.MenuItems.Models;
using TasteTrailUserExperience.Core.MenuItems.Repositories;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Infrastructure.MenuItemLikes.Services;

public class MenuItemLikeService : IMenuItemLikeService
{
    private readonly IMenuItemLikeRepository _menuItemLikeRepository;
    
    private readonly IMenuItemRepository _menuItemRepository;

    public MenuItemLikeService(IMenuItemLikeRepository menuItemLikeRepository, IMenuItemRepository menuItemRepository)
    {
        _menuItemLikeRepository = menuItemLikeRepository;
        _menuItemRepository = menuItemRepository;
    }

    public async Task<int> CreateMenuItemLikeAsync(MenuItemLikeCreateDto menuItemLikeCreateDto, User user)
    {
        var menuItemLikeToCreate = new MenuItemLike()
        {
            MenuItemId = menuItemLikeCreateDto.MenuItemId,
            UserId = user.Id,
        };

        var exists = await _menuItemLikeRepository.Exists(menuItemLikeToCreate.MenuItemId, menuItemLikeToCreate.UserId);

        if (exists)
            throw new InvalidOperationException($"You already liked this menuItem.");

        var id = await _menuItemLikeRepository.CreateAsync(menuItemLikeToCreate);

        var menuItemId = await _menuItemRepository.IncrementLikesAsync(new MenuItem()
        {
            Name = "",
            MenuId = 0,
            Id = menuItemLikeToCreate.MenuItemId,
            UserId = user.Id,
        });

        if(menuItemId is null)
        {
            await _menuItemLikeRepository.DeleteByIdAsync(id);
        }

        return id;
    }

    public async Task<int?> DeleteMenuItemLikeByIdAsync(int menuItemId, User user)
    {
        if (menuItemId <= 0)
            throw new ArgumentException($"Invalid ID value: {menuItemId}.");

        var menuItemLikeToDelete = await _menuItemLikeRepository.GetByMenuItemAndUserId(menuItemId, user.Id);

        if (menuItemLikeToDelete is null)
            return null;

        if (menuItemLikeToDelete.UserId != user.Id)
            throw new ForbiddenAccessException();

        var menuItemLikeId = await _menuItemLikeRepository.DeleteByIdAsync(menuItemLikeToDelete.Id);
        var menuItemIdToDecrementLikes = await _menuItemRepository.DecrementLikesAsync(new MenuItem()
        {
            Name = "",
            MenuId = 0,
            Id = menuItemLikeToDelete.MenuItemId,
            UserId = user.Id,
        });

        if(menuItemIdToDecrementLikes is null)
        {
            await _menuItemLikeRepository.CreateAsync(new MenuItemLike()
            {
                MenuItemId = menuItemLikeToDelete.MenuItemId,
                UserId = user.Id,
            });
        }

        return menuItemLikeId;
    }
}
