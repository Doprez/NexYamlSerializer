﻿using Microsoft.CodeAnalysis;
using NexYamlSourceGenerator.MemberApi.Analyzers;

namespace NexYamlSourceGenerator.MemberApi.PropertyAnalyzers;

internal class IsArray(IMemberSymbolAnalyzer<IPropertySymbol> analyzer) : MemberSymbolAnalyzer<IPropertySymbol>(analyzer)
{
    public override bool AppliesTo(Data<IPropertySymbol> context)
    {
        return context.Symbol.Type.TypeKind == TypeKind.Array;
    }
}