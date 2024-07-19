using RestSharp;
using RestSharp.Authenticators;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Infrastructure.Email;

public class EmailSender : IEmailService
{
    private readonly IConfiguration _config;
    private readonly string _apiKey;
    private readonly string _domain;
    public EmailSender(IConfiguration config)
    {
        _config = config;
        _apiKey = _config["Mailgun:ApiKey"];
        _domain = _config["Mailgun:Domain"];
    }

    public async Task<bool> SendEmailAsync(string userEmail, string emailSubject, string msg)
    {
        var options = new RestClientOptions
        {
            BaseUrl = new Uri($"https://api.mailgun.net/v3/{_domain}"),
            Authenticator = new HttpBasicAuthenticator("api", _apiKey)
        };

        var client = new RestClient(options);

        var request = new RestRequest("messages", Method.Post);
        request.AddParameter("from", $"BioTS <mailgun@{_domain}>");
        request.AddParameter("to", userEmail);
        request.AddParameter("subject", emailSubject);
        request.AddParameter("text", msg);

        var response = await client.ExecuteAsync(request);
        // if (!response.IsSuccessful)
        // {
        //     throw new Exception($"Failed to send email: {response.ErrorMessage}");
        // }
        return response.IsSuccessful;
    }

    public async Task SendGridEmailAsync(string userEmail, string emailSubject, string msg)
    {
        var client = new SendGridClient(_config["SendGrid:Key"]);
        var message = new SendGridMessage
        {
            From = new EmailAddress("biotrackingserbia@outlook.com", _config["Sendgrid:User"]),
            Subject = emailSubject,
            PlainTextContent = msg,
            HtmlContent = msg
        };
        message.AddTo(new EmailAddress(userEmail));
        message.SetClickTracking(false, false);

        await client.SendEmailAsync(message);
    }
}