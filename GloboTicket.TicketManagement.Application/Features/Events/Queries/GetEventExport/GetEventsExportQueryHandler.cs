﻿using AutoMapper;
using GloboTicket.TicketManagement.Application.Contracts.Infrastcture;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GloboTicket.TicketManagement.Application.Features.Events.Queries.GetEventExport;
public class GetEventsExportQueryHandler : IRequestHandler<GetEventsExportQuery, EventExportFileVm>
{
    private readonly IMapper _mapper;
    private readonly IAsyncRepository<Event> _eventRepository;
    private readonly ICsvExporter _csvExporter;

    public GetEventsExportQueryHandler(IMapper mapper,
        IAsyncRepository<Event> eventRepository, ICsvExporter csvExporter)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
        _csvExporter = csvExporter;
    }

    public async Task<EventExportFileVm> Handle(GetEventsExportQuery request, CancellationToken cancellationToken)
    {
        var allEvents = _mapper.Map<List<EventExportDto>>(
            (await _eventRepository.ListAllAsync())
            .OrderBy(e => e.Date));

        var fileData = _csvExporter.ExportEventsToCsv(allEvents);

        var eventExportFileVm = new EventExportFileVm()
        {
            ContentType = "text/csv",
            Data = fileData,
            EventExportFileName = $"{Guid.NewGuid()}.csv"
        };

        return eventExportFileVm;
    }
}
