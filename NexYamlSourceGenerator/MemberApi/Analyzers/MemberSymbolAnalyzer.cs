﻿
using Microsoft.CodeAnalysis;
using NexYamlSourceGenerator.MemberApi.Data;
namespace NexYamlSourceGenerator.MemberApi.Analyzers;
internal abstract class MemberSymbolAnalyzer<T>(IMemberSymbolAnalyzer<T> analyzer) : IMemberSymbolAnalyzer<T>
    where T : ISymbol
{
    protected readonly IMemberSymbolAnalyzer<T> _analyzer = analyzer;

    public virtual SymbolInfo Analyze(MemberData<T> symbol)
    {
        if (AppliesTo(symbol))
            return _analyzer.Analyze(symbol);
        else
            return CreateInfo();
    }
    public abstract bool AppliesTo(MemberData<T> symbol);
    protected virtual SymbolInfo CreateInfo() => SymbolInfo.Empty;
}
