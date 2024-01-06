﻿
using Microsoft.CodeAnalysis;
using NexYamlSourceGenerator.MemberApi;

internal interface IMemberSymbolAnalyzer<T>
    where T : ISymbol
{
    public bool AppliesTo(Data<T> symbol);
    SymbolInfo Analyze(Data<T> symbol);
}