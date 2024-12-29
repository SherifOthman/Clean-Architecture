using AutoMapper;
using FluentValidation.Results;
using GloboTicket.TicketManagement.Application.Contracts.Infrastcture;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using GloboTicket.TicketManagement.Application.Models.Mail;
using GloboTicket.TicketManagement.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;



namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IMapper _mapper;
    private readonly IEventRepository _eventRepository;
    private readonly IEmailService _emailService;
    private readonly ILogger<CreateEventCommandHandler> _logger;

    public CreateEventCommandHandler(IMapper mapper, IEventRepository eventRepository,
        IEmailService emailService, ILogger<CreateEventCommandHandler> logger)
    {
        _mapper = mapper;
        _eventRepository = eventRepository;
        _emailService = emailService;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        //var validator = new CreateEventCommandValidator(_eventRepository);
        //ValidationResult validationResult = await validator.ValidateAsync(request);

        //if (validationResult.Errors.Count > 0)
        //    throw new Exceptions.ValidationException(validationResult);

        var @event = _mapper.Map<Event>(request);
        @event = await _eventRepository.AddAsync(@event);

        //Sending email notification to admin address
        await SendEmai(request, @event.EventId);
        return @event.EventId;
    }

    private async Task SendEmai(CreateEventCommand request, Guid eventId)
    {
        var email = new Email
        {
            To = "Sherif_Ali@gmail.com",
            Body = $"A new event was created {request}",
            Subject = "A new event was created"
        };

        try
        {
            await _emailService.SendEmail(email);
        }
        catch (Exception ex)
        {
            //this shouldn't stop the API from doing else so this can be logged
            _logger.LogError("Mailing about event #{eventId} failed due to" +
                " an error with the mail service: {exception}", eventId, ex);
        }
    }
}
