using GloboTicket.TicketManagement.Application.Models.Mail;

namespace GloboTicket.TicketManagement.Application.Contracts.Infrastcture;
public interface IEmailService
{
    Task<bool> SendEmail(Email email);
}
