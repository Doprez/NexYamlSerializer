using Microsoft.CodeAnalysis;
using NexYamlSourceGenerator.Core;
using NexYamlSourceGenerator.MemberApi.UniversalAnalyzers;
using System.Runtime.Serialization;

namespace NexYamlSourceGenerator.MemberApi;

internal record DataMemberContext
{
    private DataMemberContext() { }
    static DataMemberContext Empty { get; } = new DataMemberContext() { Exists = false };
    internal static DataMemberContext Create(ISymbol symbol, INamedTypeSymbol dataMemberIgnoreAttribute)
    {
        DataMemberContext context = new DataMemberContext();

        if (symbol.TryGetAttribute(dataMemberIgnoreAttribute, out AttributeData attributeData))
        {
            return Empty;
        }
        else
        {
            context.Exists = true;
            context.Mode = MemberMode.Assign;
            context.Order = 0;
        }
        return context;
    }
    public bool Exists { get; set; }
    public MemberMode Mode { get; set; }
    public int Order { get; set; }
}