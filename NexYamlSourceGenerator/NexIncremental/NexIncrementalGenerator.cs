﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis.CSharp;
using NexYamlSourceGenerator.NexAPI;
using NexYamlSourceGenerator.Core;
using NexYamlSourceGenerator.MemberApi.FieldAnalyzers;
using NexYamlSourceGenerator.MemberApi.Analysation.PropertyAnalyzers;

namespace NexYamlSourceGenerator.NexIncremental
{
    [Generator]
    internal class NexIncrementalGenerator : IIncrementalGenerator
    {
        const string DataContract = "Stride.Core.DataContractAttribute";

        private static SourceCreator SourceCreator = new SourceCreator();
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            IncrementalValuesProvider<ClassPackage> classProvider = context.SyntaxProvider.ForAttributeWithMetadataName(DataContract,
                (node, transform) =>
                {
                    return node is TypeDeclarationSyntax;
                },
                (ctx, transform) =>
                {
                    var classDeclaration = (ITypeSymbol)ctx.TargetSymbol;
                    IMemberSelector recursiveMemberSelector = new MemberSelector();
                    SemanticModel semanticModel = ctx.SemanticModel;
                    Compilation compilation = semanticModel.Compilation;
                    ReferencePackage package = new ReferencePackage(compilation);
                    if (!package.IsValid())
                        return null;
                    return new ClassSymbolConverter().Convert(classDeclaration, recursiveMemberSelector, package);
                }
            );

            context.RegisterSourceOutput(classProvider, Generate);
        }

        private void Generate(SourceProductionContext context, ClassPackage info)
        {
            context.AddSource(info.ClassInfo.GeneratorName + ".g.cs", SourceCreator.Create(context, info));
        }
    }
}
internal class ClassSymbolConverter
{
    internal ClassPackage Convert(ITypeSymbol namedTypeSymbol, IMemberSelector selector, ReferencePackage package)
    {
        IMemberSymbolAnalyzer<IPropertySymbol> standardAssignAnalyzer = new PropertyAnalyzer()
            .HasVisibleGetter()
            .HasVisibleSetter();
        IMemberSymbolAnalyzer<IFieldSymbol> standardFieldAssignAnalyzer = new FieldAnalyzer()
            .IsVisibleToSerializer();

        MemberProcessor classInfoMemberProcessor = new MemberProcessor(selector, package)
            .Attach(standardAssignAnalyzer)
            .Attach(standardFieldAssignAnalyzer);
        ImmutableList<SymbolInfo> members = classInfoMemberProcessor.Process(namedTypeSymbol);

        return new ClassPackage()
        {
            ClassInfo = ClassInfo.CreateFrom(namedTypeSymbol),
            MemberSymbols = members,
        };
    }
}