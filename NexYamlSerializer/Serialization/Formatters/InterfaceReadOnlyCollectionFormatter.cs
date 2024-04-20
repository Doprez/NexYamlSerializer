#nullable enable
using NexVYaml.Parser;
using Stride.Core;
using System.Collections.Generic;

namespace NexVYaml.Serialization;

public class InterfaceReadOnlyCollectionFormatter<T> : YamlSerializer<IReadOnlyCollection<T>?>
{
    public override IReadOnlyCollection<T>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            parser.Read();
            return default;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);

        var list = new List<T>();
        while (!parser.End && parser.CurrentEventType != ParseEventType.SequenceEnd)
        {
            var value = default(T);
            context.DeserializeWithAlias(ref parser, ref value);
            list.Add(value!);
        }

        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return list!;
    }

    public override void Serialize(ISerializationWriter stream, IReadOnlyCollection<T>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(style);

        foreach (var x in value)
        {
            stream.Write(x);
        }

        stream.EndSequence();
    }
}
