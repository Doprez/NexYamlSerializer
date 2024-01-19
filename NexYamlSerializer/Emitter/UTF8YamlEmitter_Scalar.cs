﻿using NexVYaml.Internal;
using NexYamlSerializer.Emitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NexVYaml.Emitter;
public partial class Utf8YamlEmitter
{

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void BeginScalar(Span<byte> output, ref int offset)
    {
        switch (StateStack.Current)
        {
            case EmitState.BlockSequenceEntry:
                {
                    // first nested element
                    if (IsFirstElement)
                    {
                        var output2 = Writer.GetSpan(FlowSequenceSeparator.Length + 1);
                        var offset2 = 0;
                        FlowSequenceSeparator.CopyTo(output2);
                        offset2 += FlowSequenceSeparator.Length;
                        output2[offset2++] = YamlCodes.FlowSequenceStart;
                        Writer.Advance(offset);
                        switch (StateStack.Previous)
                        {
                            case EmitState.BlockSequenceEntry:
                                IndentationManager.IncreaseIndent();
                                output[offset++] = YamlCodes.Lf;
                                break;
                            case EmitState.BlockMappingValue:
                                output[offset++] = YamlCodes.Lf;
                                break;
                        }
                    }
                    WriteIndent(output, ref offset);
                    BlockSequenceEntryHeader.CopyTo(output[offset..]);
                    offset += BlockSequenceEntryHeader.Length;

                    // Write tag
                    if (tagStack.TryPop(out var tag))
                    {
                        offset += StringEncoding.Utf8.GetBytes(tag, output[offset..]);
                        output[offset++] = YamlCodes.Lf;
                        WriteIndent(output, ref offset);
                    }
                    break;
                }
            case EmitState.FlowSequenceEntry:
                if (StateStack.Previous is not EmitState.BlockMappingValue)
                    break;
                if(IsFirstElement)
                {
                    if (tagStack.TryPop(out var tag))
                    {
                        offset += StringEncoding.Utf8.GetBytes(tag, output[offset..]);
                        output[offset++] = YamlCodes.Space;
                    }
                    FlowSequenceEntryHeader.CopyTo(output[offset..]);
                    offset += FlowSequenceEntryHeader.Length;
                }
                else
                {
                    FlowSequenceSeparator.CopyTo(output[offset..]);
                    offset += FlowSequenceSeparator.Length;
                    break;
                }
                break;
            case EmitState.BlockMappingKey:
                {
                    if (IsFirstElement)
                    {
                        switch (StateStack.Previous)
                        {
                            case EmitState.BlockSequenceEntry:
                                {
                                    IndentationManager.IncreaseIndent();

                                    // Try write tag
                                    if (tagStack.TryPop(out var tag))
                                    {
                                        offset += StringEncoding.Utf8.GetBytes(tag, output[offset..]);
                                        output[offset++] = YamlCodes.Lf;
                                        WriteIndent(output, ref offset);
                                    }
                                    else
                                    {
                                        WriteIndent(output, ref offset, Options.IndentWidth - 2);
                                    }
                                    // The first key in block-sequence is like so that: "- key: .."
                                    break;
                                }
                            case EmitState.BlockMappingValue:
                                {
                                    IndentationManager.IncreaseIndent();
                                    // Try write tag
                                    if (tagStack.TryPop(out var tag))
                                    {
                                        offset += StringEncoding.Utf8.GetBytes(tag, output[offset..]);
                                    }
                                    output[offset++] = YamlCodes.Lf;
                                    WriteIndent(output, ref offset);
                                    break;
                                }
                            default:
                                WriteIndent(output, ref offset);
                                break;
                        }

                        // Write tag
                        if (tagStack.TryPop(out var tag2))
                        {
                            offset += StringEncoding.Utf8.GetBytes(tag2, output[offset..]);
                            output[offset++] = YamlCodes.Lf;
                            WriteIndent(output, ref offset);
                        }
                    }
                    else
                    {
                        WriteIndent(output, ref offset);
                    }
                    break;
                }
            case EmitState.BlockMappingValue:
                break;
            case EmitState.FlowMappingValue: break;
            case EmitState.FlowMappingKey:
                if(IsFirstElement)
                {
                    if (tagStack.TryPop(out var tag))
                    {
                        offset += StringEncoding.Utf8.GetBytes(tag, output[offset..]);
                        output[offset++] = YamlCodes.Space;
                        WriteIndent(output, ref offset);
                    }
                    FlowMappingStart.CopyTo(output[offset..]);
                    offset += 2;
                }
                if(!IsFirstElement)
                {
                    FlowSequenceSeparator.CopyTo(output[offset..]);
                    offset += FlowSequenceSeparator.Length;
                }
                break;
            case EmitState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(StateStack.Current.ToString());
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void EndScalar(Span<byte> output, ref int offset)
    {
        switch (StateStack.Current)
        {
            case EmitState.BlockSequenceEntry:
                output[offset++] = YamlCodes.Lf;
                currentElementCount++;
                break;
            case EmitState.BlockMappingKey:
                MappingKeyFooter.CopyTo(output[offset..]);
                offset += MappingKeyFooter.Length;
                StateStack.Current = EmitState.BlockMappingValue;
                break;
            case EmitState.FlowMappingKey:
                MappingKeyFooter.CopyTo(output[offset..]);
                offset += MappingKeyFooter.Length;
                StateStack.Current = EmitState.FlowMappingValue;
                break;
            case EmitState.FlowMappingValue:
                StateStack.Current = EmitState.FlowMappingKey;
                currentElementCount++;
                break;
            case EmitState.BlockMappingValue:
                output[offset++] = YamlCodes.Lf;
                StateStack.Current = EmitState.BlockMappingKey;
                currentElementCount++;
                break;
            case EmitState.FlowSequenceEntry:
                currentElementCount++;
                break;
            case EmitState.None:
                break;
            default:
                throw new ArgumentOutOfRangeException(StateStack.Current.ToString());
        }
        Writer.Advance(offset);
    }
}
