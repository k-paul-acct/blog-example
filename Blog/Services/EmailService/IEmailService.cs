namespace Blog.Services.EmailService;

public interface IEmailService
{
    Task<IResult> SendEmailAsync(string to, string subject, string body, bool isHtml = false);
}