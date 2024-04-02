#nullable enable
using NexVYaml;
using NexVYaml.Emitter;
using NexVYaml.Parser;
using Stride.Core;

namespace NexVYaml.Serialization;

public class BooleanFormatter : YamlSerializer<bool>, IYamlFormatter<bool>
{
    public static readonly BooleanFormatter Instance = new();

    public void Serialize(ref Utf8YamlEmitter emitter, bool value, YamlSerializationContext context, DataStyle style = DataStyle.Normal)
    {
        emitter.WriteBool(value);
    }

    public override bool Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        var result = parser.GetScalarAsBool();
        parser.Read();
        return result;
    }

    public override void Serialize(ref IYamlStream stream, bool value, DataStyle style = DataStyle.Normal)
    {
        stream.Write(ref value, style);
    }
}
