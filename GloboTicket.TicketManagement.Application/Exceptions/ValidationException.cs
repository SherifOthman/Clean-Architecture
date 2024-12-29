using FluentValidation.Results;


namespace GloboTicket.TicketManagement.Application.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<string> ValidationErrors { get; set; }
   // public List<string> ValidationErrors { get; set; }

    public ValidationException(ValidationResult validationResult)
    {
        ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage);
        //ValidationErrors = new List<string>();
        //validationResult.Errors.ForEach(x => ValidationErrors.Add(x.ErrorMessage));
    }

    public ValidationException(IEnumerable< ValidationResult> validationResults)
    {
        ValidationErrors = validationResults.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
    }

}