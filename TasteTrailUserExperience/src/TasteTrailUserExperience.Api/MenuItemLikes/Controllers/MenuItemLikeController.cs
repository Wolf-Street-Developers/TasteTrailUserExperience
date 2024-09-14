using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.MenuItemLikes.Dtos;
using TasteTrailUserExperience.Core.MenuItemLikes.Services;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Api.MenuItemLikes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MenuItemLikeController : ControllerBase
{
    private readonly IMenuItemLikeService _menuItemLikeService;

    public MenuItemLikeController(IMenuItemLikeService menuItemLikeService)
    {
        _menuItemLikeService = menuItemLikeService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromForm] MenuItemLikeCreateDto menuItemLike)
    {
        try
        {
            var user =  new User() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };
            
            var menuItemLikeId = await _menuItemLikeService.CreateMenuItemLikeAsync(menuItemLike, user);

            return Ok(menuItemLikeId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteByMenuItemIdAsync(int menuItemId)
    {
        try
        {
            var user =  new User() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };

            var menuItemLikeId = await _menuItemLikeService.DeleteMenuItemLikeByIdAsync(menuItemId, user);

            if (menuItemLikeId is null)
                return NotFound(menuItemLikeId);

            return Ok(menuItemLikeId);
        }
        catch (ForbiddenAccessException)
        {
            return Forbid();
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }
}
