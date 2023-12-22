﻿using Microsoft.CodeAnalysis;

namespace NexYamlSourceGenerator.MemberApi.UniversalAnalyzers;

internal class WhenOne<T>(
        IMemberSymbolAnalyzer<T> analyzer
        , Func<IMemberSymbolAnalyzer<T>, IMemberSymbolAnalyzer<T>> first
        , Func<IMemberSymbolAnalyzer<T>, IMemberSymbolAnalyzer<T>> second
    ) : MemberSymbolAnalyzer<T>(analyzer)
    where T : ISymbol
{
    public override bool AppliesTo(MemberContext<T> symbol)
    {
        if (first.Invoke(_analyzer).AppliesTo(symbol))
            return true;
        if (second.Invoke(_analyzer).AppliesTo(symbol))
            return true;
        return false;
    }
}