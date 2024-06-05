using EventManagementApp.Areas.Admin.Models;
using EventManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApp.Controllers;

public class EventsController(IUnitOfWork uof, ILogger<EventsController> logger) : Controller
{
    private ILogger<EventsController> _logger = logger;
    private IUnitOfWork _uof = uof;
    public async Task<IActionResult> Index([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        var contentType = Request.ContentType;
        if (contentType == "application/json")
        {
            if (start == null || end == null)
            {
                return Ok(new
                {
                    events = new List<Event>()
                });
            }

            var events = await _uof.EventRepository.GetEventsByDateRange(start, end);
            return Ok(new
            {
                events
            });
        }
        return View();
    }




}
