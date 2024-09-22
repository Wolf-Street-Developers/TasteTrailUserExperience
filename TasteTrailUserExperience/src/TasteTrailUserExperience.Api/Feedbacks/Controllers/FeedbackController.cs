using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailData.Infrastructure.Filters.Dtos;
using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.Feedbacks.Dtos;
using TasteTrailUserExperience.Core.Feedbacks.Services;
using TasteTrailUserExperience.Core.Users.Dtos;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Api.Feedbacks.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class FeedbackController : ControllerBase
{
    private readonly IFeedbackService _feedbackService;

    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    [HttpPost]
    public async Task<IActionResult> GetFilteredByVenueAsync([FromBody] FilterParametersDto filterParameters, [FromQuery] int venueId)
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
            
            var filterResponse = await _feedbackService.GetFeedbacksFilteredAsync(filterParameters, venueId, user);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> GetFilteredAsync([FromBody] FilterParametersSearchDto filterParameters)
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

            var filterResponse = await _feedbackService.GetFeedbacksFilteredAsync(filterParameters, user);

            return Ok(filterResponse);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpGet("{feedbackId}")]
    public async Task<IActionResult> GetByIdAsync(int feedbackId)
    {
        try
        {
            var feedback = await _feedbackService.GetFeedbackByIdAsync(feedbackId);

            if (feedback is null)
                return NotFound(feedbackId);

            return Ok(feedback);
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
            var count = await _feedbackService.GetFeedbacksCountAsync();

            return Ok(count);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromBody] FeedbackCreateDto feedback)
    {
        try
        {
            var user =  new User() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };

            var feedbackId = await _feedbackService.CreateFeedbackAsync(feedback, user);

            return Ok(feedbackId);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return this.InternalServerError(ex.Message);
        }
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteByIdAsync([FromQuery] int venueId)
    {
        try
        {
            var user =  new UserDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };

            var feedbackId = await _feedbackService.DeleteFeedbackByIdAsync(venueId, user);

            if (feedbackId is null)
                return NotFound(feedbackId);

            return Ok(feedbackId);
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

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> UpdateAsync([FromBody] FeedbackUpdateDto feedback)
    {
        try
        {
            var user =  new UserDto() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Role = User.FindFirst(ClaimTypes.Role)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };

            var feedbackId = await _feedbackService.PutFeedbackAsync(feedback, user);

            if (feedbackId is null)
                return NotFound(feedbackId);

            return Ok(feedbackId);
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

}
