using System.Net;

using Amazon.S3;
using Amazon.S3.Model;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Sisa.Abstractions;
using Sisa.Infrastructure.Settings;


namespace Sisa.Infrastructure.Services;

/// <inheritdoc/>
public class FileStorageService(
    IAmazonS3 s3Client,
    IOptions<AwsSettings> options,
    ILogger<FileStorageService> logger
) : IFileStorageService
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly IOptions<AwsSettings> _options = options;
    private readonly ILogger<FileStorageService> _logger = logger;

    /// <inheritdoc/>
    public string GetPresignedUrl(string bucket, string filePath, TimeSpan? expiration)
    {
        _logger.LogInformation($"Getting Presigned URL for object from bucket {bucket} with file path {filePath}");

        var request = new GetPreSignedUrlRequest
        {
            BucketName = bucket,
            Key = filePath,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.Add(expiration ?? TimeSpan.FromMinutes(5))
        };

        try
        {
            var url = _s3Client.GetPreSignedURL(request);

            _logger.LogInformation($"Successfully got Presigned URL for object from bucket {bucket} with file path {filePath}");

            return url;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to get Presigned URL for object from bucket {bucket} with file path {filePath}");

            throw;
        }
    }

    /// <inheritdoc/>
    public async ValueTask<Stream?> DownloadAsync(string bucket, string filePath, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Downloading object from bucket {bucket} with file path {filePath}");

        var request = new GetObjectRequest
        {
            BucketName = bucket,
            Key = filePath
        };

        try
        {
            var response = await _s3Client.GetObjectAsync(request);

            if (response is not null && response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully downloaded object from bucket {bucket} with file path {filePath}");

                using var responseStream = new MemoryStream();
                await response.ResponseStream.CopyToAsync(responseStream, cancellationToken);
                responseStream.Seek(0, SeekOrigin.Begin);

                return responseStream;
            }

            _logger.LogError($"Failed to download object from bucket {bucket} with file path {filePath}");

            return null;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to download object from bucket {bucket} with file path {filePath}");

            throw;
        }
    }

    /// <inheritdoc/>
    public async ValueTask<IUploadResponse?> UploadAsync(
        string bucket, string path, string fileName,
        Stream stream, string contentType, IDictionary<string, string> tags,
        CancellationToken cancellationToken = default)
    {
        string newFileName = Guid.NewGuid().ToString();
        string originalName = fileName;
        string extension = Path.GetExtension(fileName)?.ToLower() ?? string.Empty;
        DateTime currentDate = DateTime.UtcNow.Date;
        string filePath = $"{path}/{currentDate:yyyy}/{currentDate:MM}/{currentDate:dd}";
        string key = $"{filePath}/{newFileName}{extension}";
        long size = stream.Length;

        _logger.LogInformation($"Uploading object to bucket {bucket} with key {key}");

        var request = new PutObjectRequest
        {
            BucketName = bucket,
            Key = key,
            InputStream = stream,
            TagSet = tags.Select(x => new Tag { Key = x.Key, Value = x.Value }).ToList(),
            ContentType = contentType,
            DisablePayloadSigning = _options.Value.DisablePayloadSigning
        };

        try
        {
            PutObjectResponse response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully uploaded object to bucket {bucket} with key {key}.");

                return new UploadResponse
                {
                    Bucket = bucket,
                    Path = path,
                    Name = newFileName,
                    Extension = extension,

                    Size = size,
                    ContentType = contentType,

                    Tags = tags,

                    OriginalName = fileName,
                    Key = key
                };
            }

            _logger.LogError($"Failed to upload object to bucket {bucket} with key {key}.");

            return null;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to upload object to bucket {bucket} with key {key}.");

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<bool> DeleteAsync(
        string bucket, string filePath,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"Deleting object from bucket {bucket} with file path {filePath}");

        var response = await _s3Client.DeleteObjectAsync(bucket, filePath);

        if (response is not null && response.HttpStatusCode == HttpStatusCode.NoContent)
        {
            _logger.LogInformation($"Successfully deleted object from bucket {bucket} with file path {filePath}");

            return true;
        }

        _logger.LogError($"Failed to delete object from bucket {bucket} with file path {filePath}");

        return false;
    }

    public async ValueTask<IUploadPartResponse?> InitMultiPartUploadAsync(string bucket, string path, string fileName, string contentType, IDictionary<string, string> tags, CancellationToken cancellationToken = default)
    {
        var newFileName = Guid.NewGuid().ToString();
        var originalName = fileName;
        var extension = Path.GetExtension(fileName)?.ToLower() ?? string.Empty;
        var currentDate = DateTime.UtcNow.Date;
        var filePath = $"{path}/{currentDate:yyyy}/{currentDate:MM}/{currentDate:dd}";
        var key = $"{filePath}/{newFileName}{extension}";

        _logger.LogInformation($"Uploading object to bucket {bucket} with key {key}");

        InitiateMultipartUploadRequest request = new()
        {
            BucketName = bucket,
            Key = key,
            ContentType = contentType,
            TagSet = []
        };

        try
        {
            InitiateMultipartUploadResponse response = await _s3Client.InitiateMultipartUploadAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully uploaded part of object to bucket {bucket} with key {key}.");

                return new UploadPartResponse
                {
                    Bucket = bucket,
                    Path = path,
                    Name = newFileName,
                    Extension = extension,

                    ContentType = contentType,

                    Tags = tags,

                    OriginalName = fileName,
                    Key = key,
                    UploadId = response.UploadId
                };
            }

            _logger.LogError($"Failed to upload part of object to bucket {bucket} with key {key}.");

            return null;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to upload part of object to bucket {bucket} with key {key}.");

            throw;
        }
    }

    public async ValueTask<IUploadPartResponse?> UploadPartAsync(string bucket, string key, Stream streamPart, string uploadId, int partNumber, CancellationToken cancellationToken = default)
    {
        var request = new UploadPartRequest()
        {
            BucketName = bucket,
            Key = key,
            UploadId = uploadId,
            PartNumber = partNumber,
            // PartSize = 5 * (long)Math.Pow(2, 20), // 5 MB
            PartSize = streamPart.Length,
            InputStream = streamPart,
            DisablePayloadSigning = _options.Value.DisablePayloadSigning
        };

        try
        {
            var response = await _s3Client.UploadPartAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully uploaded part ({partNumber}) of object to bucket {bucket} with key {key}.");

                return new UploadPartResponse()
                {
                    Bucket = bucket,
                    Key = key,

                    UploadId = uploadId,
                    PartNumber = partNumber,
                    ETag = response.ETag,
                };
            }

            _logger.LogError($"Failed to upload part ({partNumber}) of object to bucket {bucket} with key {key}.");

            return null;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to upload part ({partNumber}) of object to bucket {bucket} with key {key}.");

            try
            {
                _logger.LogInformation($"Aborting multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

                await AbortMultipartUploadAsync(bucket, key, uploadId, cancellationToken);

            }
            catch (AmazonS3Exception abortEx)
            {
                _logger.LogError(abortEx, $"Failed to abort multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

                throw;
            }

            throw;
        }
    }

    public async ValueTask<bool> CompleteMultiPartUploadAsync(string bucket, string key, string uploadId, Dictionary<int, string> eTags, CancellationToken cancellationToken = default)
    {
        var request = new CompleteMultipartUploadRequest()
        {
            BucketName = bucket,
            Key = key,
            UploadId = uploadId,
            PartETags = eTags.Select(x => new PartETag(x.Key, x.Value)).ToList()
        };

        try
        {
            var response = await _s3Client.CompleteMultipartUploadAsync(request, cancellationToken);

            // https://docs.aws.amazon.com/AmazonS3/latest/API/API_CompleteMultipartUpload.html
            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"Successfully completed multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

                return true;
            }

            _logger.LogError($"Failed to complete multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

            return response.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to complete multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

            try
            {
                _logger.LogInformation($"Aborting multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

                await AbortMultipartUploadAsync(bucket, key, uploadId, cancellationToken);

            }
            catch (AmazonS3Exception abortEx)
            {
                _logger.LogError(abortEx, $"Failed to abort multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

                throw;
            }

            throw;
        }
    }

    public async ValueTask<bool> AbortMultipartUploadAsync(string bucket, string key, string uploadId, CancellationToken cancellationToken = default)
    {
        var request = new AbortMultipartUploadRequest()
        {
            BucketName = bucket,
            Key = key,
            UploadId = uploadId,
        };

        try
        {
            var response = await _s3Client.AbortMultipartUploadAsync(request, cancellationToken);

            // https://docs.aws.amazon.com/AmazonS3/latest/API/API_AbortMultipartUpload.html
            if (response.HttpStatusCode == HttpStatusCode.NoContent)
            {
                _logger.LogInformation($"Successfully aborted multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

                return true;
            }

            _logger.LogError($"Failed to abort multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

            return false;
        }
        catch (AmazonS3Exception ex)
        {
            _logger.LogError(ex, $"Failed to abort multipart upload ({uploadId}) of object to bucket {bucket} with key {key}.");

            throw;
        }
    }
}

public record UploadResponse : IUploadResponse
{
    public string Bucket { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public IDictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    public string ContentType { get; set; } = string.Empty;

    public string OriginalName { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
}

public record UploadPartResponse : IUploadPartResponse
{
    public string Bucket { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Extension { get; set; } = string.Empty;
    public long Size { get; set; }
    public IDictionary<string, string> Tags { get; set; } = new Dictionary<string, string>();
    public string ContentType { get; set; } = string.Empty;

    public string OriginalName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;

    public string UploadId { get; set; } = string.Empty;
    public int PartNumber { get; set; }
    public string Key { get; set; } = string.Empty;
    public string ETag { get; set; } = string.Empty;
}