﻿using System;
using System.Linq;

namespace NexYamlSerializer.NewYaml;
public interface IYamlStream
{
    void Serialize(ref char value);
    void Serialize(ref string? value);
    void Serialize(ReadOnlySpan<byte> value);
}
