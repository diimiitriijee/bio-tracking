namespace Application.Interfaces;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string userEmail, string emailSubject, string msg);
    Task SendGridEmailAsync(string userEmail, string emailSubject, string msg);
}