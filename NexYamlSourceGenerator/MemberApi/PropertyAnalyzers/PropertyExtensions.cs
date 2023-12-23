﻿using Microsoft.CodeAnalysis;
using NexYamlSourceGenerator.MemberApi.PropertyAnalyzers;

namespace NexYamlSourceGenerator.MemberApi.Analysation.PropertyAnalyzers;
internal static class PropertyExtensions
{
   internal static IMemberSymbolAnalyzer<IPropertySymbol> HasOriginalDefinition(this IMemberSymbolAnalyzer<IPropertySymbol> propertySymbolAnalyzer, INamedTypeSymbol originalDefinition)
        => new ValidatorOriginalDefinition(propertySymbolAnalyzer, originalDefinition);
    internal static IMemberSymbolAnalyzer<IPropertySymbol> HasVisibleGetter(this IMemberSymbolAnalyzer<IPropertySymbol> propertySymbolAnalyzer)
        => new HasVisibleGetter(propertySymbolAnalyzer);
    internal static IMemberSymbolAnalyzer<IPropertySymbol> HasVisibleSetter(this IMemberSymbolAnalyzer<IPropertySymbol> propertySymbolAnalyzer)
        => new HasVisibleSetter(propertySymbolAnalyzer);
    internal static IMemberSymbolAnalyzer<IPropertySymbol> IsArray(this IMemberSymbolAnalyzer<IPropertySymbol> propertySymbolAnalyzer)
        => new IsArray(propertySymbolAnalyzer);
    internal static IMemberSymbolAnalyzer<IPropertySymbol> IsByteArray(this IMemberSymbolAnalyzer<IPropertySymbol> propertySymbolAnalyzer)
        => new IsByteArray(propertySymbolAnalyzer);
}