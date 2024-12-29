
using FluentValidation;
using FluentValidation.Results;
using GloboTicket.TicketManagement.Application.Exceptions;
using MediatR;

namespace GloboTicket.TicketManagement.Application.PipelineBehaviors;
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>

{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (_validators.Any())
        {
            var validationContext = new ValidationContext<TRequest>(request);

           // var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            //var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

            //if (failures.Count > 0)
            //    throw new Exceptions.ValidationException(new ValidationResult(failures));

            var validationResults = (await Task.WhenAll(_validators.Select(v => v.ValidateAsync(validationContext, cancellationToken))))
                .Where(v => !v.IsValid);


            if (validationResults.Any())
            {
                throw new Exceptions.ValidationException(validationResults);
            }

        }

        return await next();

    }
}