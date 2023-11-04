namespace Sisa.Abstractions;

public interface IEmailSenderService
{
    Task<bool> SendWithEmbeddedTemplateAsync<TModel>(
        string to, string subject,
        TModel model, string templateName,
        CancellationToken cancellationToken = default);
}
