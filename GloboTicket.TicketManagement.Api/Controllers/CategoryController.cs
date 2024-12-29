using GloboTicket.TicketManagement.Application.Features.Categories.Commands.CreateCategory;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesList;
using GloboTicket.TicketManagement.Application.Features.Categories.Queries.GetCategoriesWithEvents;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace GloboTicket.TicketManagement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("all", Name = "GetAllCategories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<List<CategoryListVm>>> GetAllCategories()
    {
        var dtos = await _mediator.Send(new GetCategoriesListQuery());
        return Ok(dtos);
    }

    [HttpGet("allwithevents", Name = "GetCategoriesWithEvents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize]
    public async Task<ActionResult<CategoryEventListVm>> GetCategoriesWithEvents(
        bool IncludeHistory)
    {
        var dtos = await _mediator.Send(new GetCategoriesListWithEventsQuery
        { IncludeHistory = IncludeHistory });

        return Ok(dtos);
    }

    [HttpPost(Name = "AddCategory")]
    [Authorize]
    public async Task<ActionResult<CreateCategoryCommandResponse>> Create(
        [FromBody] CreateCategoryCommand categoryCommand)
    {
        var response = await _mediator.Send(categoryCommand);

        return Ok(response);
    }
}
