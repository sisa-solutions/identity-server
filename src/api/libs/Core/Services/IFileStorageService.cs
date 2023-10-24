namespace Sisa.Abstractions;

/// <summary>
/// The interface for the object storage service.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Gets the presigned URL for the object from the specified bucket and key.
    /// </summary>
    /// <param name="bucket">The name of the bucket containing the object to get the presigned URL for.</param>
    /// <param name="filePath">The key of the object to get the presigned URL for.</param>
    /// <param name="expiration">The expiration of the presigned URL.</param>
    /// <returns>The presigned URL for the object.</returns>
    string GetPresignedUrl(string bucket, string filePath, TimeSpan? expiration);

    /// <summary>
    /// Downloads the object from the specified bucket and key.
    /// </summary>
    /// <param name="bucket">The name of the bucket containing the object to download.</param>
    /// <param name="filePath">The key of the object to download.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The stream of the object.</returns>
    ValueTask<Stream?> DownloadAsync(string bucket, string filePath, CancellationToken cancellationToken = default);

    /// <summary>
    /// Uploads the object to the specified bucket and key.
    /// </summary>
    /// <param name="bucket">The name of the bucket to upload the object to.</param>
    /// <param name="path">The path of the object to upload.</param>
    /// <param name="fileName">The path of the object to upload, included extension.</param>
    /// <param name="stream">The stream of the object to upload.</param>
    /// <param name="contentType">The content type of the object to upload.</param>
    /// <param name="tags">The tags of the object to upload.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the object was uploaded successfully, false otherwise.</returns>
    ValueTask<IUploadResponse?> UploadAsync(string bucket, string path, string fileName, Stream stream, string contentType, IDictionary<string, string> tags, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the object from the specified bucket and key.
    /// </summary>
    /// <param name="bucket">The name of the bucket containing the object to delete.</param>
    /// <param name="filePath">The key of the object to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>True if the object was deleted successfully, false otherwise.</returns>
    ValueTask<bool> DeleteAsync(string bucket, string filePath, CancellationToken cancellationToken = default);

    ValueTask<IUploadPartResponse?> InitMultiPartUploadAsync(string bucket, string path, string fileName, string contentType, IDictionary<string, string> tags, CancellationToken cancellationToken = default);

    ValueTask<IUploadPartResponse?> UploadPartAsync(string bucket, string key, Stream streamPart, string uploadId, int partNumber, CancellationToken cancellationToken = default);
    ValueTask<bool> CompleteMultiPartUploadAsync(string bucket, string key, string uploadId, Dictionary<int, string> eTags, CancellationToken cancellationToken = default);

    ValueTask<bool> AbortMultipartUploadAsync(string bucket, string key, string uploadId, CancellationToken cancellationToken = default);
}

public interface IUploadResponse
{
    string Bucket { get; }
    string Name { get; }
    string Extension { get; }
    long Size { get; }
    IDictionary<string, string> Tags { get; }
    string ContentType { get; }

    string OriginalName { get; }
    string Key { get; }
}

public interface IUploadPartResponse
{
    string UploadId { get; }
    int PartNumber { get; }
    string Name { get; }
    string Key { get; }
    string ETag { get; }
}

