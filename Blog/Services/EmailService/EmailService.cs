using System.Net;
using System.Net.Mail;
using Blog.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.Services.EmailService;

public class EmailService : IEmailService
{
    private readonly SmtpClient _client;
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettingsOptions)
    {
        _smtpSettings = smtpSettingsOptions.Value;
        _client = new SmtpClient(_smtpSettings.Server, _smtpSettings.Port)
        {
            Credentials = new NetworkCredential(_smtpSettings.Email, _smtpSettings.Password),
            EnableSsl = true
        };
    }

    public async Task<IResult> SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        var message = new MailMessage(_smtpSettings.Email, to, subject, body);
        try
        {
            await _client.SendMailAsync(message);
            return Results.Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.Problem();
        }
    }
}