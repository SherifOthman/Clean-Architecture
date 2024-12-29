using GloboTicket.TicketManagement.Application.Contracts.Infrastcture;
using GloboTicket.TicketManagement.Application.Models.Mail;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace GloboTicket.TicketManagement.Infrastructure.Mail;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;
    private readonly ILogger<EmailSettings> _logger;

    public EmailService(IOptionsMonitor<EmailSettings> emailSettings,
        ILogger<EmailSettings> logger)
    {
        _emailSettings = emailSettings.CurrentValue;
        _logger = logger;
    }

    public async Task<bool> SendEmail(Email email)
    {

        var client = new SendGridClient(_emailSettings.ApiKey);

        var from = new EmailAddress()
        {
            Email = _emailSettings.FromAddress,
            Name = _emailSettings.FromName
        };

        var sendGrindMessage = MailHelper.CreateSingleEmail(from, new EmailAddress(email.To),
            email.Subject, email.Body, email.Body);

        var response = await client.SendEmailAsync(sendGrindMessage);

        _logger.LogInformation("Email sent");

        if (response.StatusCode == System.Net.HttpStatusCode.Accepted ||
                response.StatusCode == System.Net.HttpStatusCode.OK)
            return true;


        _logger.LogError("Email sending failed");

        return false;
    }
}
