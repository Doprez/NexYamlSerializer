#nullable enable
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Unicode;
using NexVYaml.Emitter;
using NexVYaml.Parser;
using NexYamlSerializer;
using NexYamlSerializer.Emitter.Serializers;
using Stride.Core;

namespace NexVYaml.Serialization;

public class DictionaryFormatter<TKey, TValue> : YamlSerializer<Dictionary<TKey, TValue>?>,IYamlFormatter<Dictionary<TKey, TValue>?>
    where TKey : notnull
{
    public override Dictionary<TKey, TValue> Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {
        if (parser.IsNullScalar())
        {
            parser.Read();
            return default;
        }
        var map = new Dictionary<TKey, TValue>();
        if (this.IsPrimitiveType(typeof(TKey)))
        {
            var keyFormatter = context.Resolver.GetFormatter<TKey>();
            parser.ReadWithVerify(ParseEventType.MappingStart);


            while (!parser.End && parser.CurrentEventType != ParseEventType.MappingEnd)
            {
                var key = context.DeserializeWithAlias(keyFormatter, ref parser);
                var value = context.DeserializeWithAlias<TValue>(ref parser);
                map.Add(key, value);
            }

            parser.ReadWithVerify(ParseEventType.MappingEnd);
            return map;
        }
        else
        {
            var listFormatter = new ListFormatter<KeyValuePair<TKey, TValue>>();
            var keyValuePairs = context.DeserializeWithAlias(listFormatter, ref parser);
            
            return keyValuePairs?.ToDictionary() ?? [];
        }
    }

    public override void Serialize(ref ISerializationWriter stream, Dictionary<TKey, TValue>? value, DataStyle style = DataStyle.Normal)
    {
        YamlSerializer<TKey> keyFormatter = null;
        YamlSerializer<TValue> valueFormatter = null;
        if (FormatterExtensions.IsPrimtiveType(typeof(TKey)))
        {
            keyFormatter = NewSerializerRegistry.Instance.GetFormatter<TKey>();
        }
        if (FormatterExtensions.IsPrimtiveType(typeof(TValue)))
            valueFormatter = NewSerializerRegistry.Instance.GetFormatter<TValue>();

        if (keyFormatter == null)
        {
            stream.Emitter.BeginSequence();
            if (value.Count > 0)
            {
                var elementFormatter = new KeyValuePairFormatter<TKey, TValue>();
                foreach (var x in value)
                {
                    elementFormatter.Serialize(ref stream, x);
                }
            }
            stream.Emitter.EndSequence();
        }
        else if (valueFormatter == null)
        {
            stream.Emitter.BeginMapping();
            {
                foreach (var x in value)
                {
                    keyFormatter.Serialize(ref stream, x.Key, style);
                    stream.Serialize(x.Value);
                }
            }
            stream.Emitter.EndMapping();
        }
        else
        {
            stream.Emitter.BeginMapping();
            {
                foreach (var x in value)
                {
                    keyFormatter.Serialize(ref stream, x.Key, style);
                    valueFormatter.Serialize(ref stream, x.Value, style);
                }
            }
            stream.Emitter.EndMapping();
        }
    }
}
file class DictionaryFormatterHelper : IYamlFormatterHelper
{
    public void Register(IYamlFormatterResolver resolver)
    {
        NewSerializerRegistry.Instance.Register(this, typeof(Dictionary<,>), typeof(Dictionary<,>));
        resolver.Register(this, typeof(Dictionary<,>), typeof(Dictionary<,>));
        NewSerializerRegistry.Instance.RegisterGenericFormatter(typeof(Dictionary<,>), typeof(DictionaryFormatter<,>));
        resolver.RegisterGenericFormatter(typeof(Dictionary<,>), typeof(DictionaryFormatter<,>));
        NewSerializerRegistry.Instance.RegisterFormatter(typeof(Dictionary<,>));
        resolver.RegisterFormatter(typeof(Dictionary<,>));

        NewSerializerRegistry.Instance.Register(this, typeof(Dictionary<,>), typeof(System.Collections.Generic.IDictionary<,>));
        resolver.Register(this, typeof(Dictionary<,>), typeof(System.Collections.Generic.IDictionary<,>));
        NewSerializerRegistry.Instance.Register(this, typeof(Dictionary<,>), typeof(System.Collections.Generic.IReadOnlyDictionary<,>));
        resolver.Register(this, typeof(Dictionary<,>), typeof(System.Collections.Generic.IReadOnlyDictionary<,>));

    }
    public IYamlFormatter Create(Type type)
    {
        if (type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IDictionary<,>))
        {
            var generatorType = typeof(DictionaryFormatter<,>);
            var genericParams = type.GenericTypeArguments;
            var param = new Type[] { genericParams[0], genericParams[1] };
            var filledGeneratorType = generatorType.MakeGenericType(param);
            return (IYamlFormatter)Activator.CreateInstance(filledGeneratorType);
        }

        if (type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IReadOnlyDictionary<,>))
        {
            var generatorType = typeof(DictionaryFormatter<,>);
            var genericParams = type.GenericTypeArguments;
            var param = new Type[] { genericParams[0], genericParams[1] };
            var filledGeneratorType = generatorType.MakeGenericType(param);
            return (IYamlFormatter)Activator.CreateInstance(filledGeneratorType);
        }


        var gen = typeof(DictionaryFormatter<,>);
        var genParams = type.GenericTypeArguments;
        var fillGen = gen.MakeGenericType(genParams);
        return (IYamlFormatter)Activator.CreateInstance(fillGen);
    }

    public YamlSerializer Instantiate(Type type)
    {
        if (type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IDictionary<,>))
        {
            var generatorType = typeof(DictionaryFormatter<,>);
            var genericParams = type.GenericTypeArguments;
            var param = new Type[] { genericParams[0], genericParams[1] };
            var filledGeneratorType = generatorType.MakeGenericType(param);
            return (YamlSerializer)Activator.CreateInstance(filledGeneratorType);
        }

        if (type.GetGenericTypeDefinition() == typeof(System.Collections.Generic.IReadOnlyDictionary<,>))
        {
            var generatorType = typeof(DictionaryFormatter<,>);
            var genericParams = type.GenericTypeArguments;
            var param = new Type[] { genericParams[0], genericParams[1] };
            var filledGeneratorType = generatorType.MakeGenericType(param);
            return (YamlSerializer)Activator.CreateInstance(filledGeneratorType);
        }


        var gen = typeof(DictionaryFormatter<,>);
        var genParams = type.GenericTypeArguments;
        var fillGen = gen.MakeGenericType(genParams);
        return (YamlSerializer)Activator.CreateInstance(fillGen);
    }
}

