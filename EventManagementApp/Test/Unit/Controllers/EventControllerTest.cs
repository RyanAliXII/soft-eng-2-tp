
using EventManagementApp.Areas.Admin.Controllers;
using Xunit;
using Moq;
using EventManagementApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using EventManagementApp.Areas.Admin.Models;
using Xunit.Abstractions;
using System.Text.Json;
using System.Text.Json.Serialization;
using EventManagementApp.Areas.Admin.ViewModels;
using EventManagementApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EventManagementApp.Tests.Unit;

public class IndexRouteJSONResponse
{
    [JsonPropertyName("events")]
    public List<Event>? Events { get; set; }
}
public class EventControllerTest
{
    private readonly ITestOutputHelper _outputHelper;

    public EventControllerTest(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }
    [Fact]
    public async Task Index_ReturnViewResult()
    {

        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var contentType = "";

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.ContentType).Returns(contentType);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object,
        };

        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };


        var result = await controller.Index(null, null);
        Assert.IsType<ViewResult>(result);


    }

    [Fact]
    public async Task Index_ReturnJSONIfContentTypeIsApplicationJSON()
    {
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var contentType = "application/json";

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.ContentType).Returns(contentType);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object,
        };

        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };

        var result = await controller.Index(null, null);
        Assert.IsType<OkObjectResult>(result);

    }
    [Fact]
    public async Task Index_ReturnEventsIfDateRangeIsDefined()
    {
        var mockEvents = MockEvents();
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetEventsByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(() => Task.FromResult(mockEvents));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);

        var contentType = "application/json";

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.ContentType).Returns(contentType);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object,
        };

        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };
        var start = new DateTime(2024, 4, 1);
        var end = new DateTime(2024, 5, 1);
        var result = await controller.Index(start, end);
        var jsonResult = Assert.IsType<OkObjectResult>(result);
        var jsonRaw = JsonSerializer.Serialize(jsonResult.Value);
        var jsonResponse = JsonSerializer.Deserialize<IndexRouteJSONResponse>(jsonRaw);
        var response = Assert.IsType<IndexRouteJSONResponse>(jsonResponse);
        Assert.IsType<List<Event>>(response?.Events);
        Assert.Equal(mockEvents.Count, response.Events.Count);
    }
    [Fact]
    public async Task Index_ReturnEmptyListEventIfStartDateIsNotDefined()
    {
        var mockEvents = MockEvents();
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetEventsByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(() => Task.FromResult(mockEvents));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);

        var contentType = "application/json";

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.ContentType).Returns(contentType);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object,
        };

        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };
        var end = new DateTime(2024, 5, 1);
        var result = await controller.Index(null, end);
        var jsonResult = Assert.IsType<OkObjectResult>(result);
        var jsonRaw = JsonSerializer.Serialize(jsonResult.Value);
        var jsonResponse = JsonSerializer.Deserialize<IndexRouteJSONResponse>(jsonRaw);
        var response = Assert.IsType<IndexRouteJSONResponse>(jsonResponse);
        Assert.IsType<List<Event>>(response.Events);
        Assert.Empty(response.Events);
    }
    [Fact]
    public async Task Index_ReturnEmptyListEventIfEndDateIsNotDefined()
    {
        var mockEvents = MockEvents();
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetEventsByDateRange(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(() => Task.FromResult(mockEvents));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);

        var contentType = "application/json";

        var mockRequest = new Mock<HttpRequest>();
        mockRequest.Setup(x => x.ContentType).Returns(contentType);

        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object,
        };

        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };
        var start = new DateTime(2024, 4, 1);
        var result = await controller.Index(start, null);
        var jsonResult = Assert.IsType<OkObjectResult>(result);
        var jsonRaw = JsonSerializer.Serialize(jsonResult.Value);
        var jsonResponse = JsonSerializer.Deserialize<IndexRouteJSONResponse>(jsonRaw);
        var response = Assert.IsType<IndexRouteJSONResponse>(jsonResponse);
        Assert.IsType<List<Event>>(response.Events);
        Assert.Empty(response.Events);
    }
    [Fact]
    public void Create_ReturnView()
    {
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = controller.Create();
        Assert.IsType<ViewResult>(result);
    }
    [Fact]
    public async Task Create_ReturnBadRequestModelStateIsInvalid()
    {
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var mockResponse = new Mock<HttpResponse>();
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);
        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };
        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };
        controller.ModelState.AddModelError("name", "Name is required.");
        var result = await controller.Create(NewEventData());
        var jsonResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(StatusCodes.Status400BadRequest, jsonResult.StatusCode);
    }

    [Fact]
    public async Task Create_ReturnOkIfModelStateIsValid()
    {
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var mockResponse = new Mock<HttpResponse>();
        var mockHttpContext = new Mock<HttpContext>();
        mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);
        var controllerContext = new ControllerContext()
        {
            HttpContext = mockHttpContext.Object
        };
        var controller = new EventController(mockUOF.Object, logger.Object)
        {
            ControllerContext = controllerContext
        };

        var result = await controller.Create(NewEventData());
        var jsonResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(StatusCodes.Status200OK, jsonResult.StatusCode);
    }

    [Fact]
    public async Task Edit_ReturnNotFoundIfInvalidParamIsInvalidGUID()
    {
        var uid = Guid.Empty;
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Edit(uid);
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public async Task Edit_ReturnNotFoundIfRecordNotExistsView()
    {
        var uid = new Guid("24af7cbb-7cde-49ec-9ec1-3e9216b25d5a");

        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetById(uid)).Returns(Task.FromResult(new Event()
        {
            Id = Guid.Empty,
            Name = "",
            Date = new DateTime(),
            Activities = []
        }));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Edit(uid);
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public async Task Edit_ReturnViewOk()
    {
        var uid = new Guid("24af7cbb-7cde-49ec-9ec1-3e9216b25d5a");

        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetById(uid)).Returns(Task.FromResult(new Event()
        {
            Id = uid,
            Name = "Sample Event",
            Date = new DateTime(),
            Activities = []
        }));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Edit(uid);
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public async Task Edit_ReturnBadRequestIfParamAndBodyIdDidNotMatch()
    {
        var uid = new Guid("24af7cbb-7cde-49ec-9ec1-3e9216b25d5a");

        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();

        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Edit(new EditEventViewModel()
        {
            Id = Guid.NewGuid(),
            Name = "Sample Event",
            Date = new DateTime(),
            Activities = [],
        }, uid);
        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Edit_ReturnBadRequestIfModelStateIsInvalid()
    {
        var uid = new Guid("24af7cbb-7cde-49ec-9ec1-3e9216b25d5a");

        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();

        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        controller.ModelState.AddModelError("name", "Name is required.");
        var result = await controller.Edit(new EditEventViewModel()
        {
            Id = uid,
            Name = "Sample Event",
            Date = new DateTime(),
            Activities = [],
        }, uid);
        Assert.IsType<BadRequestObjectResult>(result);
    }
    [Fact]
    public async Task Edit_ReturnOk()
    {
        var uid = new Guid("24af7cbb-7cde-49ec-9ec1-3e9216b25d5a");
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetById(uid)).Returns(Task.FromResult(new Event()
        {
            Id = uid,
            Name = "Sample Event",
            Date = new DateTime(),
            Activities = []
        }));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Edit(new EditEventViewModel()
        {
            Id = uid,
            Name = "Edited event",
            Date = new DateTime(),
            Activities = [],
        }, uid);
        Assert.IsType<OkObjectResult>(result);

    }
    [Fact]
    public async Task Delete_ReturnBadRequestIfParamIsInvalidGuid()
    {
        var uid = Guid.Empty;
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Delete(uid);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnBadRequestIfEventNotExists()
    {
        var uid = Guid.NewGuid();
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetById(uid)).Returns(Task.FromResult(new Event()
        {
            Id = Guid.Empty,
            Name = "",
            Date = new DateTime(),
            Activities = []
        }));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Delete(uid);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnOk()
    {
        var uid = Guid.NewGuid();
        var logger = new Mock<ILogger<EventController>>();
        var mockUOF = new Mock<IUnitOfWork>();
        var mockEventRepo = new Mock<IEventRepository>();
        mockEventRepo.Setup(x => x.GetById(uid)).Returns(Task.FromResult(new Event()
        {
            Id = uid,
            Name = "Event to delete.",
            Date = new DateTime(),
            Activities = []
        }));
        mockUOF.Setup(x => x.EventRepository).Returns(mockEventRepo.Object);
        var controller = new EventController(mockUOF.Object, logger.Object);
        var result = await controller.Delete(uid);
        Assert.IsType<OkObjectResult>(result);
    }
    private List<Event> MockEvents()
    {
        return new List<Event>()
        {
            new Event
            {
                Name = "Event 1",
                Date = new DateTime(),
                Activities = []

            },
            new Event{
                Name = "Event 2",
                Date = new DateTime(),
                Activities = []

            },
            new Event{
                Name = "Event 3",
                Date = new DateTime(),
                Activities = []

            }
        };
    }
    private NewEventViewModel NewEventData()
    {
        return new NewEventViewModel
        {
            Name = "",
            Date = new DateTime(),
            Activities = []

        };
    }
}
