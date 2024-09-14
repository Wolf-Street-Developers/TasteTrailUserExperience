using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.MenuItems.Services;
using TasteTrailUserExperience.Core.Users.Models;
namespace TasteTrailUserExperience.Api.MenuItems.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MenuItemController : ControllerBase
{
    private readonly IMenuItemService _menuItemService;


    public MenuItemController(IMenuItemService menuItemService)
    {
        _menuItemService = menuItemService;
    }

    [HttpPost("{menuId}")]
    public async Task<IActionResult> GetFilteredAsync(FilterParametersSearchDto filterParameters, int menuId)
    {
        try 
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            User? user = null;

            if (id is not null || role is not null || username is not null)
            {
                user =  new User() 
                {
                    Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                    Username = User.FindFirst(ClaimTypes.Name)!.Value
                };
            }
            
            var filterResponse = await _menuItemService.GetMenuItemsFilteredAsync(filterParameters, menuId, user);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetFilteredAsync(FilterParametersSearchDto filterParameters)
    {
        try 
        {
            var id = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            User? user = null;

            if (id is not null || role is not null || username is not null)
            {
                user =  new User() 
                {
                    Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                    Username = User.FindFirst(ClaimTypes.Name)!.Value
                };
            }

            var filterResponse = await _menuItemService.GetMenuItemsFilteredAsync(filterParameters, user);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
             var menuItem = await _menuItemService.GetMenuItemByIdAsync(id);

            if (menuItem is null)
                return NotFound(id);

            return Ok(menuItem);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }
}
