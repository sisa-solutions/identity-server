// using System.Reflection;

// namespace Sisa.Abstractions.Services;

// public interface IEmailSender
// {
//     Task<bool> SendWithEmbeddedTemplateAsync<TModel>(
//         string to, string subject,
//         TModel model, Assembly assembly, string templateName,
//         CancellationToken cancellationToken = default);

//     Task<IDictionary<string, bool>> SendWithEmbeddedTemplateAsync<TModel>(
//         string subject,
//         IDictionary<string, TModel> models,
//         Assembly assembly, string templateName,
//         CancellationToken cancellationToken = default);
// }
