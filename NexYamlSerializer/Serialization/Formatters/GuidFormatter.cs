#nullable enable
using NexVYaml.Parser;
using Stride.Core;
using System;
using System.Buffers.Text;

namespace NexVYaml.Serialization;

public class GuidFormatter : YamlSerializer<Guid>
{
    public static readonly GuidFormatter Instance = new();

    protected override void Write(IYamlWriter stream, Guid value, DataStyle style)
    {
        // nnnnnnnn-nnnn-nnnn-nnnn-nnnnnnnnnnnn
        Span<byte> buf = stackalloc byte[64];
        if (Utf8Formatter.TryFormat(value, buf, out var bytesWritten))
        {
            ReadOnlySpan<byte> bytes = buf[..bytesWritten];
            stream.Serialize(bytes);
        }
        else
        {
            throw new YamlException($"Cannot serialize {value}");
        }
    }

    protected override void Read(YamlParser parser, YamlDeserializationContext context, ref Guid value)
    {
        if (parser.TryGetScalarAsSpan(out var span) &&
              Utf8Parser.TryParse(span, out Guid guid, out var bytesConsumed) &&
              bytesConsumed == span.Length)
        {
            parser.Read();
            value = guid;
        }
        throw new YamlException($"Cannot detect a scalar value of Guid : {parser.CurrentEventType} {parser.GetScalarAsString()}");
    }
}
