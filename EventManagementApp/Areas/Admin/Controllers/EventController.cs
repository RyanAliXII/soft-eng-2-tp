using EventManagementApp.Areas.Admin.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EventManagementApp.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class EventController(ILogger<EventController> logger) : Controller
{
    private ILogger<EventController> _logger = logger;
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([FromBody] NewEventViewModel newEvent)
    {
        try
        {

            ValidateOverlappingTime(newEvent.Activities);
            if (!ModelState.IsValid)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new
                {
                    errors = ModelState.ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors?.Select(e => e.ErrorMessage)?.ToArray() ?? []
                 )
                });
            }

            return Json(new
            {
                Id = ""
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Json(new
            {
                errors = new { },
            });
        }

    }
    private void ValidateOverlappingTime(List<NewActivityViewModel> activities)
    {
        activities = [.. activities.Select((a, index) =>
        {
            a.Index = index;
            return a;
        }).OrderBy(a => a.StartTime)];

        for (int i = 0; i < activities.Count; i++)
        {
            var current = activities[i];

            if (current.StartTime.Equals(current.EndTime))
            {
                ModelState.AddModelError($"activities[{current.Index}].startTime", "Start and end time cannot be equal.");
                ModelState.AddModelError($"activities[{current.Index}].endTime", "Start and end time cannot be equal.");
            }
            if (current.StartTime > current.EndTime)
            {
                ModelState.AddModelError($"activities[{current.Index}].startTime", "Start time cannot be greater than end time");
            }
            if (activities.Count == i + 1) break;
            var next = activities[i + 1];
            if (current.EndTime > next.StartTime)
            {
                ModelState.AddModelError($"activities[{current.Index}].startTime", "Time overlaps with another activity.");
                ModelState.AddModelError($"activities[{current.Index}].endTime", "Time overlaps with another activity.");
                ModelState.AddModelError($"activities[{next.Index}].startTime", "Time overlaps with another activity.");
                ModelState.AddModelError($"activities[{next.Index}].endTime", "Time overlaps with another activity.");
            }
        }
    }
}