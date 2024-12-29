using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventList;

public class GetEventListQueryHandler : IRequestHandler<GetEventListQuery, List<EventListVm>>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Event> _eventRepository;

    public GetEventListQueryHandler(IMapper mapper, IAsyncRepository<Event> eventRepository)
    {

        _mapper = mapper;
        _eventRepository = eventRepository;
    }

    public async Task<List<EventListVm>> Handle(GetEventListQuery request, CancellationToken cancellationToken)
    {
        var allEvents = (await _eventRepository.ListAllAsync()).OrderBy(x => x.Date);
        return _mapper.Map<List<EventListVm>>(allEvents);
    }
}
