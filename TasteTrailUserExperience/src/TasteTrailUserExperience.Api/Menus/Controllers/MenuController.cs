using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Menus.Services;

namespace TasteTrailUserExperience.Api.Menus.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MenuController : ControllerBase
{
    private readonly IMenuService _menuService;

    public MenuController(IMenuService menuService)
    {
        _menuService = menuService;
    }

    [HttpPost]
    public async Task<IActionResult> GetFilteredByVenueAsync([FromBody] PaginationParametersDto paginationParameters, [FromQuery] int venueId)
    {
        try 
        {
            var filterResponse = await _menuService.GetMenusFilteredAsync(paginationParameters, venueId);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetFilteredAsync([FromBody] PaginationSearchParametersDto paginationSearchParameters)
    {
        try 
        {
            var filterResponse = await _menuService.GetMenusFilteredAsync(paginationSearchParameters);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpGet("{menuId}")]
    public async Task<IActionResult> GetByIdAsync(int menuId)
    {
        try
        {
            var menu = await _menuService.GetMenuByIdAsync(menuId);

            if (menu is null)
                return NotFound(menuId);

            return Ok(menu);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }
}
