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
    public async Task<IActionResult> GetFilteredAsync([FromBody] PaginationParametersDto paginationParameters, int venueId)
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
    public async Task<IActionResult> GetFilteredAsync(PaginationSearchParametersDto paginationSearchParameters)
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

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync(int id)
    {
        try
        {
            var menu = await _menuService.GetMenuByIdAsync(id);

            if (menu is null)
                return NotFound(id);

            return Ok(menu);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }
}
