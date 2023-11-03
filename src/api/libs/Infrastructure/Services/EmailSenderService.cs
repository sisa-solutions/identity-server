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

        var templatePath = assembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith(templateName));

        if (templatePath == null)
        {
            _logger.LogError("Template {Template} not found", templateName);

            throw new InvalidOperationException($"Template {templateName} not found");
        }

        using var scope = _serviceProvider.CreateScope();
        var _fluentEmail = scope.ServiceProvider.GetRequiredService<IFluentEmail>();

        _logger.LogInformation("Sending email to {To} with subject {Subject}", to, subject);

        var result = await _fluentEmail
            .To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded(templatePath, model, assembly)
            .SendAsync(cancellationToken);

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

    public async Task<IDictionary<string, bool>> SendWithEmbeddedTemplateAsync<TModel>(
        string subject,
        IDictionary<string, TModel> models,
        string templateName,
        CancellationToken cancellationToken = default)
    {
        var assembly = typeof(TModel).Assembly;

        var templatePath = assembly.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith(templateName));

        if (templatePath == null)
        {
            _logger.LogError("Template {Template} not found", templateName);

            throw new InvalidOperationException($"Template {templateName} not found");
        }

        using var scope = _serviceProvider.CreateScope();
        var _fluentEmailFactory = scope.ServiceProvider.GetRequiredService<IFluentEmailFactory>();

        Func<string, IFluentEmail> getFluentEmail = (to) => _fluentEmailFactory
            .Create()
            .To(to)
            .Subject(subject)
            .UsingTemplateFromEmbedded(templatePath, models[to], assembly);

        var fluentEmailTasks = models
            .Select(async x =>
            {
                var fluentEmail = getFluentEmail(x.Key);

                var result = await fluentEmail.SendAsync(cancellationToken);

                return new KeyValuePair<string, bool>(x.Key, result.Successful);
            });

        var results = await Task.WhenAll(fluentEmailTasks);

        return results.ToDictionary(x => x.Key, x => x.Value);
    }
}
