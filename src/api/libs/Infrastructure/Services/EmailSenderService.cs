using FluentEmail.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Sisa.Abstractions;

namespace Sisa.Infrastructure.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EmailSenderService> _logger;

    public EmailSenderService(
        IServiceProvider serviceProvider,
        ILogger<EmailSenderService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task<bool> SendWithEmbeddedTemplateAsync<TModel>(
        string to, string subject,
        TModel model, string templateName,
        CancellationToken cancellationToken = default)
    {
        var assembly = typeof(TModel).Assembly;

        var htmlTemplatePath = assembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith($"{templateName}.cshtml"));

        if (htmlTemplatePath == null)
        {
            _logger.LogError("Template {Template} not found", templateName);

            throw new InvalidOperationException($"Template {templateName} not found");
        }

        var txtTemplatePath = assembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith($"{templateName}.txt"));

        using var scope = _serviceProvider.CreateScope();
        var fluentEmail = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

        _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);

        fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded(htmlTemplatePath, model, assembly);

        if (txtTemplatePath != null)
        {
            fluentEmail.PlaintextAlternativeUsingTemplateFromEmbedded(txtTemplatePath, model, assembly);
        }

        var result = await fluentEmail.SendAsync(cancellationToken);

        if (result.Successful)
        {
            _logger.LogInformation("Email sent to {To} with subject {Subject}", to, subject);
        }
        else
        {
            _logger.LogError("Email to {To} with subject {Subject} failed: {@Result}", to, subject, result);
        }

        return result.Successful;
    }
}
