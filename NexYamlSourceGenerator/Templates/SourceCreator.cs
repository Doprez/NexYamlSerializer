﻿using Microsoft.CodeAnalysis;
using NexYamlSourceGenerator.MemberApi.Data;
using System.Text;

namespace NexYamlSourceGenerator.Templates;

internal static class SourceCreator
{
    internal static string ConvertToSourceCode(this ClassPackage package)
    {
        var info = package.ClassInfo;
        var ns = info.NameSpace != null ? "namespace " + info.NameSpace + ";" : "";
        var tempVariables = new StringBuilder();
        foreach (var member in package.MemberSymbols)
        {
            tempVariables.AppendLine($"var temp_{member.Name} = default({member.Type});");
        }
        var tag = package.ClassInfo.AliasTag?.Length == 0 ?
            $"{info.NameSpace}.{info.TypeName},{{AssemblyName}}" :
            $"{package.ClassInfo.AliasTag}";
        return @$"// <auto-generated/>
//  This code was generated by Strides YamlSerializer.
//  Do not edit this file.

#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using NexVYaml;
using NexVYaml.Emitter;
using NexVYaml.Parser;
using NexVYaml.Serialization;
{ns}
[System.CodeDom.Compiler.GeneratedCode(""NexVYaml"",""1.0.0.0"")]
file class {info.GeneratorName + "Helper" } : IYamlFormatterHelper
{{
    static readonly string AssemblyName = typeof({info.ShortDefinition}).Assembly.GetName().Name;

    public void Register(IYamlFormatterResolver resolver)
    {{
{package.CreateRegisterThis()}
{package.CreateRegisterAbstracts()}
{package.CreateRegisterInterfaces()}
    }}
}}
[System.CodeDom.Compiler.GeneratedCode(""NexVYaml"",""1.0.0.0"")]
file class {info.GeneratorName + info.TypeParameterArguments} : IYamlFormatter<{info.NameDefinition}> {info.TypeParameterRestrictions}
{{

    {package.CreateUTF8Members()}
    static readonly string AssemblyName = typeof({info.ShortDefinition}).Assembly.GetName().Name;
    string IdentifierTag {{ get; }} = typeof({info.ShortDefinition}).Name;
    Type IdentifierType {{ get; }} = typeof({info.ShortDefinition});

    public void Serialize(ref Utf8YamlEmitter emitter, {info.NameDefinition} value, YamlSerializationContext context)
    {{
{package.NullCheck()}
        emitter.{package.BeginMappingStyle()};
        if(context.IsRedirected || context.IsFirst)
        {{
            emitter.Tag($""!{tag}"");
            context.IsRedirected = false;
            context.IsFirst = false;
        }}
{package.CreateSerializationEmit()}
        emitter.EndMapping();
    }}

    public {info.NameDefinition}{(info.TypeKind == TypeKind.Struct ? "" : "?")} Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {{
{package.CreateDeserialize()}
    }}
}}
";
    }
}