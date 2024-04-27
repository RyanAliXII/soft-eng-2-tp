using EventManagementApp.Areas.Admin.Models;
using EventManagementApp.Areas.Admin.ViewModels;
using EventManagementApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EventManagementApp.Areas.Admin.Controllers;

[Authorize]
[Area("Admin")]
public class EventController(IUnitOfWork uof, ILogger<EventController> logger) : Controller
{
    private ILogger<EventController> _logger = logger;
    private IUnitOfWork _uof = uof;

    public async Task<IActionResult> Index([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var contentType = Request.ContentType;
        if (contentType == "application/json")
        {
            if (start == null || end == null)
            {
                return Json(new
                {
                    events = new List<Event>()
                });
            }

            var events = await _uof.EventRepository.GetEventsByDateRange(start, end);
            return Json(new
            {
                events
            });
        }
        return View();
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromBody] NewEventViewModel newEvent)
    {
        try
        {
            var dateExists = await _uof.EventRepository.IsDateExists(newEvent.Date);
            if (dateExists)
            {
                ModelState.AddModelError("date", "Event with this date already exists.");
            }
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
            var e = await _uof.EventRepository.Add(new Event(newEvent));
            await _uof.Save();
            return Json(new
            {
                e
            });

        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Json(new
            {
                errors = new { },
            });
        }

    }
    public async Task<IActionResult> Edit(Guid ID)
    {
        if (ID == Guid.Empty)
        {
            return NotFound();
        }
        var e = await _uof.EventRepository.GetById(ID);
        if (e.Id == Guid.Empty)
        {
            return NotFound();
        }
        return View(e);
    }
    [HttpPut]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromBody] EditEventViewModel editEventVM, Guid ID)
    {
        try
        {
            if (ID == Guid.Empty || ID != editEventVM.Id)
            {
                Response.StatusCode = StatusCodes.Status400BadRequest;
                return Json(new
                {
                    message = "Invalid event ID param",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var dateExists = await _uof.EventRepository.IsDateExistsExceptWithId(editEventVM.Date, ID);
            if (dateExists)
            {
                ModelState.AddModelError("date", "Event with this date already exists.");
            }
            ValidateOverlappingTime(editEventVM.Activities);
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
            var dbEvent = await _uof.EventRepository.GetById(editEventVM.Id);
            dbEvent.Update(editEventVM);
            _uof.EventRepository.Update(dbEvent);
            await _uof.Save();
            return Json(new
            {
                status = StatusCodes.Status200OK,
                message = "Event updated.",
                @event = dbEvent
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            _logger.LogError(ex.StackTrace);
            Response.StatusCode = StatusCodes.Status500InternalServerError;
            return Json(new
            {
                message = "Unknown error occured.",
                status = StatusCodes.Status500InternalServerError,
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
    private void ValidateOverlappingTime(List<EditActivityViewModel> activities)
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