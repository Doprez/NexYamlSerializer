#nullable enable
using NexVYaml.Parser;
using Stride.Core;
using System;

namespace NexVYaml.Serialization;

public class TupleFormatter<T1> : YamlSerializer<Tuple<T1>?>
{
    public override Tuple<T1>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1>(item1);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2> : YamlSerializer<Tuple<T1, T2>?>
{
    public override Tuple<T1, T2>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2>(item1, item2);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2, T3> : YamlSerializer<Tuple<T1, T2, T3>?>
{
    public override Tuple<T1, T2, T3>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        var item3 = context.DeserializeWithAlias<T3>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2, T3>(item1, item2, item3);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2, T3>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.Serialize(value.Item3, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2, T3, T4> : YamlSerializer<Tuple<T1, T2, T3, T4>?>
{
    public override Tuple<T1, T2, T3, T4>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        var item3 = context.DeserializeWithAlias<T3>(ref parser);
        var item4 = context.DeserializeWithAlias<T4>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2, T3, T4>(item1, item2, item3, item4);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2, T3, T4>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.Serialize(value.Item3, style);
        stream.Serialize(value.Item4, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2, T3, T4, T5> : YamlSerializer<Tuple<T1, T2, T3, T4, T5>?>
{
    public override Tuple<T1, T2, T3, T4, T5>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        var item3 = context.DeserializeWithAlias<T3>(ref parser);
        var item4 = context.DeserializeWithAlias<T4>(ref parser);
        var item5 = context.DeserializeWithAlias<T5>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2, T3, T4, T5>(item1, item2, item3, item4, item5);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2, T3, T4, T5>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.Serialize(value.Item3, style);
        stream.Serialize(value.Item4, style);
        stream.Serialize(value.Item5, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2, T3, T4, T5, T6> : YamlSerializer<Tuple<T1, T2, T3, T4, T5, T6>?>
{
    public override Tuple<T1, T2, T3, T4, T5, T6>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        var item3 = context.DeserializeWithAlias<T3>(ref parser);
        var item4 = context.DeserializeWithAlias<T4>(ref parser);
        var item5 = context.DeserializeWithAlias<T5>(ref parser);
        var item6 = context.DeserializeWithAlias<T6>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2, T3, T4, T5, T6>(item1, item2, item3, item4, item5, item6);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2, T3, T4, T5, T6>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.Serialize(value.Item3, style);
        stream.Serialize(value.Item4, style);
        stream.Serialize(value.Item5, style);
        stream.Serialize(value.Item6, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2, T3, T4, T5, T6, T7> : YamlSerializer<Tuple<T1, T2, T3, T4, T5, T6, T7>?>
{
    public override Tuple<T1, T2, T3, T4, T5, T6, T7>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        var item3 = context.DeserializeWithAlias<T3>(ref parser);
        var item4 = context.DeserializeWithAlias<T4>(ref parser);
        var item5 = context.DeserializeWithAlias<T5>(ref parser);
        var item6 = context.DeserializeWithAlias<T6>(ref parser);
        var item7 = context.DeserializeWithAlias<T7>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2, T3, T4, T5, T6, T7>(item1, item2, item3, item4, item5, item6, item7);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2, T3, T4, T5, T6, T7>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.Serialize(value.Item3, style);
        stream.Serialize(value.Item4, style);
        stream.Serialize(value.Item5, style);
        stream.Serialize(value.Item6, style);
        stream.Serialize(value.Item7, style);
        stream.EndSequence();
    }
}

public class TupleFormatter<T1, T2, T3, T4, T5, T6, T7, T8> : YamlSerializer<Tuple<T1, T2, T3, T4, T5, T6, T7, T8>?>
    where T8 : notnull
{
    public override Tuple<T1, T2, T3, T4, T5, T6, T7, T8>? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            return null;
        }

        parser.ReadWithVerify(ParseEventType.SequenceStart);
        var item1 = context.DeserializeWithAlias<T1>(ref parser);
        var item2 = context.DeserializeWithAlias<T2>(ref parser);
        var item3 = context.DeserializeWithAlias<T3>(ref parser);
        var item4 = context.DeserializeWithAlias<T4>(ref parser);
        var item5 = context.DeserializeWithAlias<T5>(ref parser);
        var item6 = context.DeserializeWithAlias<T6>(ref parser);
        var item7 = context.DeserializeWithAlias<T7>(ref parser);
        var item8 = context.DeserializeWithAlias<T8>(ref parser);
        parser.ReadWithVerify(ParseEventType.SequenceEnd);
        return new Tuple<T1, T2, T3, T4, T5, T6, T7, T8>(item1, item2, item3, item4, item5, item6, item7, item8);
    }

    public override void Serialize(ref ISerializationWriter stream, Tuple<T1, T2, T3, T4, T5, T6, T7, T8>? value, DataStyle style = DataStyle.Normal)
    {
        stream.BeginSequence(DataStyle.Compact);
        stream.Serialize(value.Item1, style);
        stream.Serialize(value.Item2, style);
        stream.Serialize(value.Item3, style);
        stream.Serialize(value.Item4, style);
        stream.Serialize(value.Item5, style);
        stream.Serialize(value.Item6, style);
        stream.Serialize(value.Item7, style);
        stream.Serialize(value.Rest, style);
        stream.EndSequence();
    }
}
