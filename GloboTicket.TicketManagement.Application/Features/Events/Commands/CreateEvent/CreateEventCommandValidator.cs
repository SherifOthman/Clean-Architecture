using FluentValidation;
using GloboTicket.TicketManagement.Application.Contracts.Persistence;
using System.Net.Mail;

namespace GloboTicket.TicketManagement.Application.Features.Events.Commands.CreateEvent;
public class CreateEventCommandValidator : AbstractValidator<CreateEventCommand>
{
    private readonly IEventRepository _eventRepository;

    public CreateEventCommandValidator(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(50).WithMessage("{ProperyName} must not exceed 50 characters");


        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .GreaterThan(DateTime.Now);

        RuleFor(e => e)
            .MustAsync(EventNameAndDateUnique)
            .WithMessage("An event with the same name and date already exists.");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .GreaterThan(0);

    }

    private async Task<bool> EventNameAndDateUnique(CreateEventCommand e, CancellationToken token)
    {
        return !(await _eventRepository.IsEventNameAndDateUnique(e.Name, e.Date));
    }
}
