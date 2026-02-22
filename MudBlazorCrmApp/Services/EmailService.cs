using Azure.Identity;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Users.Item.SendMail;

namespace MudBlazorCrmApp.Services;

public class EmailSettings
{
    public string? UserId { get; set; }
    public string? TenantId { get; set; }
    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? SendFromUserId { get; set; }
    public string? CcRecipient { get; set; }
}

public class EmailService
{
    private readonly EmailSettings _settings;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration config, ILogger<EmailService> logger)
    {
        _settings = config.GetSection("EmailSettings").Get<EmailSettings>() ?? new EmailSettings();
        _logger = logger;
    }

    public bool IsConfigured =>
        !string.IsNullOrEmpty(_settings.TenantId) &&
        !string.IsNullOrEmpty(_settings.ClientId) &&
        !string.IsNullOrEmpty(_settings.ClientSecret) &&
        !string.IsNullOrEmpty(_settings.SendFromUserId);

    public async Task SendEmailAsync(string to, string subject, string body, string? htmlBody = null, List<(string FileName, byte[] Content, string ContentType)>? attachments = null)
    {
        if (!IsConfigured)
            throw new InvalidOperationException("Email settings are not configured");

        var options = new TokenCredentialOptions { AuthorityHost = AzureAuthorityHosts.AzurePublicCloud };
        var credential = new ClientSecretCredential(_settings.TenantId, _settings.ClientId, _settings.ClientSecret, options);
        var scopes = new[] { "https://graph.microsoft.com/.default" };
        var graphClient = new GraphServiceClient(credential, scopes);

        var toRecipients = to.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(email => new Recipient { EmailAddress = new EmailAddress { Address = email } })
            .ToList();

        var ccRecipients = new List<Recipient>();
        if (!string.IsNullOrEmpty(_settings.CcRecipient))
        {
            ccRecipients.Add(new Recipient { EmailAddress = new EmailAddress { Address = _settings.CcRecipient } });
        }

        var graphAttachments = new List<Attachment>();
        if (attachments != null)
        {
            foreach (var (fileName, content, contentType) in attachments)
            {
                graphAttachments.Add(new FileAttachment
                {
                    OdataType = "#microsoft.graph.fileAttachment",
                    Name = fileName,
                    ContentBytes = content,
                    ContentType = contentType,
                });
            }
        }

        var message = new Message
        {
            Subject = subject,
            Body = new ItemBody
            {
                ContentType = htmlBody != null ? BodyType.Html : BodyType.Text,
                Content = htmlBody ?? body,
            },
            ToRecipients = toRecipients,
            CcRecipients = ccRecipients.Count > 0 ? ccRecipients : null,
            Attachments = graphAttachments.Count > 0 ? graphAttachments : null,
        };

        var sendMailRequest = new SendMailPostRequestBody
        {
            Message = message,
            SaveToSentItems = true,
        };

        await graphClient.Users[_settings.SendFromUserId].SendMail.PostAsync(sendMailRequest);
        _logger.LogInformation("Email sent to {To} with subject '{Subject}'", to, subject);
    }
}
