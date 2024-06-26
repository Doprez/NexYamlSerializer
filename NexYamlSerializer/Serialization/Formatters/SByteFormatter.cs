#nullable enable
using NexVYaml;
using NexVYaml.Parser;
using NexVYaml.Serialization;
using NexYamlSerializer.Parser;
using Stride.Core;
using System;
using System.Buffers.Text;
using System.Globalization;

namespace NexYamlSerializer.Serialization.PrimitiveSerializers;

public class SByteFormatter : YamlSerializer<sbyte>
{
    public static readonly SByteFormatter Instance = new();

    protected override void Write(IYamlWriter stream, sbyte value, DataStyle style)
    {
        Span<byte> span = stackalloc byte[4];
        value.TryFormat(span, out var written, default, CultureInfo.InvariantCulture);
        stream.Serialize(span[..written]);
    }

    protected override void Read(YamlParser parser, YamlDeserializationContext context, ref sbyte value)
    {
        if (parser.TryGetScalarAsSpan(out var span))
        {
            if (int.TryParse(span, CultureInfo.InvariantCulture, out var result))
            {
                value = checked((sbyte)result);
                parser.Read();
                return;
            }
            else if (FormatHelper.TryDetectHex(span, out var hexNumber))
            {
                if (Utf8Parser.TryParse(hexNumber, out value, out var bytesConsumed, 'x') &&
                       bytesConsumed == hexNumber.Length)
                {
                    value = checked((sbyte)result);
                    parser.Read();
                    return;
                }
            }
        }
    }
}
