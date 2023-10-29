using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Sisa.Helpers;

public static class CertificateHelper
{
    private const string BEGIN_PRIVATE_KEY = "BEGIN PRIVATE KEY";
    private const string BEGIN_RSA_PRIVATE_KEY = "BEGIN RSA PRIVATE KEY";

    public static X509Certificate2 GetCertificate<TAssembly>(string keyFile = "key.key", string certFile = "cert.crt")
    {
        Assembly currentAssembly = typeof(TAssembly).Assembly;
        string currentNamespace = typeof(TAssembly).Namespace ?? string.Empty;

        using Stream? certFileStream = currentAssembly.GetManifestResourceStream($"{currentNamespace}.{certFile}");
        using Stream? keyFileStream = currentAssembly.GetManifestResourceStream($"{currentNamespace}.{keyFile}");

        if (certFileStream == null || keyFileStream == null)
#pragma warning disable S112 // General exceptions should never be thrown
            throw new Exception("Resource not found");
#pragma warning restore S112 // General exceptions should never be thrown

        using MemoryStream certStream = new();
        certFileStream.CopyTo(certStream);

        using MemoryStream keyStream = new();
        keyFileStream.CopyTo(keyStream);

        return CreateCertificateFromStream(certStream.ToArray(), keyStream.ToArray());
    }

    public static async Task<X509Certificate2> GetCertificateAsync(string certFile = "certFile.crt", string keyFile = "keyFile.key")
    {
        if (!File.Exists(certFile))
            throw new FileNotFoundException(certFile);

        if (!File.Exists(keyFile))
            throw new FileNotFoundException(keyFile);


        var certFileBytes = await File.ReadAllBytesAsync(certFile);
        var keyFileBytes = await File.ReadAllBytesAsync(keyFile);

        if (certFileBytes.Length == 0 || keyFileBytes.Length == 0)
#pragma warning disable S112 // General exceptions should never be thrown
            throw new Exception("Key not found");
#pragma warning restore S112 // General exceptions should never be thrown

        return CreateCertificateFromStream(certFileBytes, keyFileBytes);
    }

    public static async Task<X509Certificate2[]> GetCertificatesFromPathAsync(string path)
    {
        if (!Directory.Exists(path))
            throw new DirectoryNotFoundException(path);

        var filePaths = Directory.GetFiles(path).OrderBy(p => p);

        if (!filePaths.Any() || filePaths.Count() % 2 != 0)
            throw new FileLoadException(path);

        var certGroups = filePaths.Select((filePath, index) => new
        {
            filePath,
            index
        }).GroupBy(group => group.index / 2, element => element.filePath);

        var createCertTasks = certGroups.Select(async group => await GetCertificateAsync(group.ElementAt(0), group.ElementAt(1)));

        return await Task.WhenAll(createCertTasks);
    }

    public static async Task<X509Certificate2[][]> GetCertificatesFromPathsAsync(params string [] paths)
    {
        var certTasks = paths.Select(GetCertificatesFromPathAsync);

        return await Task.WhenAll(certTasks);
    }

    private static X509Certificate2 CreateCertificateFromStream(byte[] certBytes, byte[] keyBytes)
    {
        X509Certificate2 cert = new(certBytes);
        string keyContent = Encoding.ASCII.GetString(keyBytes);
        string[] keyContentBlocks = keyContent.Split("-", StringSplitOptions.RemoveEmptyEntries);
        byte[] keyContentBytes = Convert.FromBase64String(keyContentBlocks[1]);

        using RSA rsa = RSA.Create();
        if (keyContentBlocks[0] == BEGIN_PRIVATE_KEY)
        {
            rsa.ImportPkcs8PrivateKey(keyContentBytes, out _);
        }
        else if (keyContentBlocks[0] == BEGIN_RSA_PRIVATE_KEY)
        {
            rsa.ImportRSAPrivateKey(keyContentBytes, out _);
        }

        X509Certificate2 certWithKey = cert.CopyWithPrivateKey(rsa);

        return new(certWithKey.Export(X509ContentType.Pkcs12));
    }
}
