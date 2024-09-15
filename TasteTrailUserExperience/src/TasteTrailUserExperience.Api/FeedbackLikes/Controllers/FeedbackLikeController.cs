using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailData.Api.Common.Extensions.Controllers;
using TasteTrailUserExperience.Core.Common.Exceptions;
using TasteTrailUserExperience.Core.FeedbackLikes.Dtos;
using TasteTrailUserExperience.Core.FeedbackLikes.Services;
using TasteTrailUserExperience.Core.Users.Models;

namespace TasteTrailUserExperience.Api.FeedbackLikes.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedbackLikeController : ControllerBase
{
    private readonly IFeedbackLikeService _feedbackLikeService;

    public FeedbackLikeController(IFeedbackLikeService feedbackLikeService)
    {
        _feedbackLikeService = feedbackLikeService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateAsync([FromForm] FeedbackLikeCreateDto feedbackLike)
    {
        try
        {
            var user =  new User() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };
            
            var feedbackLikeId = await _feedbackLikeService.CreateFeedbackLikeAsync(feedbackLike, user);

            return Ok(feedbackLikeId);
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

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteByFeedbackIdAsync(int feedbackId)
    {
        try
        {
            var user =  new User() {
                Id = User.FindFirst(ClaimTypes.NameIdentifier)!.Value,
                Username = User.FindFirst(ClaimTypes.Name)!.Value
            };

            var feedbackLikeId = await _feedbackLikeService.DeleteFeedbackLikeByIdAsync(feedbackId, user);

            if (feedbackLikeId is null)
                return NotFound(feedbackLikeId);

            return Ok(feedbackLikeId);
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
