using Google.Protobuf;

using Grpc.Core;

namespace Sisa.Grpc;

public interface IGrpcFileStreamInfo
{
    string Name { get; }
    string? Title { get; }
    string? Description { get; }
    Dictionary<string, string> Tags { get; }
}

public interface IGrpcFileStreamCommand<T>
    where T : class, IGrpcFileStreamInfo
{
    T Info { get; }

    ByteString Content { get; }
}

public class GrpcFileStream<TCommand> : Stream
    where TCommand : class, IGrpcFileStreamCommand<IGrpcFileStreamInfo>
{
    private readonly IAsyncStreamReader<TCommand> _requestStream;
    private byte[] _buffer = [];
    private int _position;
    private int _length;

    public GrpcFileStream(IAsyncStreamReader<TCommand> requestStream)
    {
        _requestStream = requestStream;
    }

    public override bool CanRead => true;

    public override bool CanSeek => false;

    public override bool CanWrite => false;

    public override long Length => _length;

    public override long Position { get => _position; set => throw new NotImplementedException(); }

    public override void Flush()
    {
        throw new NotImplementedException();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        if (_buffer == null || _position == _length)
        {
            // Read the next chunk from the request stream
            if (!_requestStream.MoveNext(default).Result) return 0; // End of stream

            var chunk = _requestStream.Current.Content;

            _buffer = chunk.ToByteArray();
            _position = 0;
            _length = _buffer.Length;
        }

        // Copy from the buffer to the destination array
        var available = _length - _position;
        var bytesToCopy = Math.Min(available, count);

        Buffer.BlockCopy(_buffer, _position, buffer, offset, bytesToCopy);
        _position += bytesToCopy;

        return bytesToCopy;
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        throw new NotImplementedException();
    }

    public override void SetLength(long value)
    {
        throw new NotImplementedException();
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        throw new NotImplementedException();
    }
}
