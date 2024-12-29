using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Exceptions;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.DeleteEvent;
internal class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    public DeleteEventCommandHandler(IAsyncRepository<Event> eventRepository)
    {
        EventRepository = eventRepository;
    }

    public IAsyncRepository<Event> EventRepository { get; }

    public async Task Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var eventToDelte = await EventRepository.GetByIdAsync(request.EventId);
        await EventRepository.DeleteAsync(eventToDelte);
    }
}
