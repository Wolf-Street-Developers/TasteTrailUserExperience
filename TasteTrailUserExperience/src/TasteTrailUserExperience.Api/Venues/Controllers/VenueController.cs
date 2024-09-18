using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Venues.Services;

namespace TasteTrailUserExperience.Api.Venues.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class VenueController : Controller
{
    private readonly IVenueService _venueService;

    public VenueController(IVenueService venueService)
    {
        _venueService = venueService;
    }

    [HttpPost]
    public async Task<IActionResult> GetFilteredAsync([FromBody] FilterParametersSearchDto filterParameters)
    {
        try 
        {
            var filterResponse = await _venueService.GetVenuesFilteredAsync(filterParameters);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByIdAsync([FromQuery] int venueId)
    {
        try
        {
             var venue = await _venueService.GetVenueByIdAsync(venueId);

            if (venue is null)
                return NotFound(venueId);

            return Ok(venue);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetCountAsync()
    {
        try
        {
             var count = await _venueService.GetVenuesCountAsync();

            return Ok(count);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }
}
