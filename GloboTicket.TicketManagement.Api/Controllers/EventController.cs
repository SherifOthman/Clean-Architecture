using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Commands.UpdateEvent;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventDetail;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;
using GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GloboTicket.TicketManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventController : ControllerBase
{
    private readonly IMediator _mediator;

    public EventController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet(Name = "GetAllEvents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<List<EventListVm>>> GetAllEvents()
    {
        var result = await _mediator.Send(new GetEventListQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}", Name = "GetEventById")]
    public async Task<ActionResult<EventDetailVm>> GetEventById(Guid id)
    {
        return Ok(await _mediator.Send(new GetEventDetailQuery() { Id = id}));
    }

    [HttpPost(Name = "AddEvent")]
    public async Task<ActionResult<Guid>> AddEvent(
        [FromBody] CreateEventCommand CreateEventCommand)
    {
        var result = await _mediator.Send(CreateEventCommand);

        return Ok(result);
    }

    [HttpPut(Name = "UpdateEvent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update([FromBody] UpdateEventCommand updateEventCommand)
    {
        await _mediator.Send(updateEventCommand);
        return NoContent();
    }

    [HttpDelete("{id}",Name = "DeleteEvent")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteEventCommand() { EventId = id });

        return NoContent();
    }

    [HttpGet("export", Name = "ExportEvents")]
    public async Task<FileResult> ExportEvents()
    {
        var fileDto = await _mediator.Send(new GetEventsExportQuery());

        return File(fileDto.Data, fileDto.ContentType, fileDto.EventExportFileName);
    }
}
