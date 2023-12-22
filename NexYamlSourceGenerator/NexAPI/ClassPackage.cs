﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace NexYamlSourceGenerator.NexAPI;
internal record ClassPackage
{
    internal ClassInfo ClassInfo { get; set; }
    internal ImmutableList<SymbolInfo> MemberSymbols { get; set; }
}
