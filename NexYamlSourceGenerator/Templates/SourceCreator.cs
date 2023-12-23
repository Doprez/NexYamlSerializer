﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NexYamlSourceGenerator.NexAPI;
using System.Text;

namespace NexYamlSourceGenerator.Templates;

internal static class SourceCreator
{
    internal static string ConvertToSourceCode(this ClassPackage package)
    {
        var info = package.ClassInfo;
        string ns = info.NameSpace != null ? "namespace " + info.NameSpace + ";" : "";
        StringBuilder tempVariables = new StringBuilder();
        foreach (var member in package.MemberSymbols)
        {
            tempVariables.AppendLine($"var temp_{member.Name} = default({member.Type});");
        }
        string Template = @$"// <auto-generated/>
//  This code was generated by Strides YamlSerializer.
//  Do not edit this file.

#nullable enable
using System;
using System.Linq;
using System.Collections.Generic;
using VYaml;
using VYaml.Emitter;
using VYaml.Parser;
using VYaml.Serialization;
{ns}
[System.CodeDom.Compiler.GeneratedCode(""NexVYaml"",""1.0.0.0"")]
internal class {info.GeneratorName + info.TypeParameterArguments} : IYamlFormatter<{info.NameDefinition}>
{{
    {package.CreateUTF8Members()}
    static readonly string AssemblyName = typeof({info.ShortDefinition}).Assembly.GetName().Name;
    string IdentifierTag {{ get; }} = typeof({info.ShortDefinition}).Name;
    Type IdentifierType {{ get; }} = typeof({info.ShortDefinition});

    {info.Accessor} static void Register()
    {{
        {package.CreateRegisterThis()}
        {package.CreateRegisterAbstracts()}
        {package.CreateRegisterInterfaces()}
    }}

    {info.Accessor} void Serialize(ref Utf8YamlEmitter emitter, {info.NameDefinition} value, YamlSerializationContext context)
    {{
        if (value is null)
        {{
            emitter.WriteNull();
            return;
        }}
        if(context.IsMappingEnabled)
            emitter.BeginMapping();
        if(context.IsRedirected || context.IsFirst)
        {{
            emitter.Tag($""!{info.NameSpace}.{info.TypeName},{{AssemblyName}}"");
            context.IsRedirected = false;
            context.IsFirst = false;
        }}
{package.CreateSerializationEmit()}
        if(context.IsMappingEnabled)
            emitter.EndMapping();
    }}

    {info.Accessor} {info.NameDefinition}? Deserialize(ref YamlParser parser, YamlDeserializationContext context)
    {{
        {package.CreateDeserialize()}
    }}
}}
";
        return Template;
    }



}