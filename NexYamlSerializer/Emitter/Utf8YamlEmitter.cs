#nullable enable
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Linq;
using System.Runtime.CompilerServices;
using NexVYaml.Emitter;
using NexVYaml.Internal;
using NexYamlSerializer.Emitter;
using Stride.Core;
using Stride.Engine;
namespace NexVYaml.Emitter
{
    public class YamlEmitterException(string message) : Exception(message)
    {
    }

    public enum EmitState
    {
        None,
        BlockSequenceEntry,
        BlockMappingKey,
        BlockMappingValue,
        FlowSequenceEntry,
        FlowMappingKey,
        FlowMappingValue,
    }
    public partial class Utf8YamlEmitter : IUtf8YamlEmitter
    {
        public int CurrentIndentLevel => IndentationManager.CurrentIndentLevel;
        public ExpandBuffer<EmitState> StateStack { get; }
        public IBufferWriter<byte> Writer { get; }
        public YamlEmitOptions Options { get; }

        bool IsFirstElement
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => currentElementCount <= 0;
        }

        IndentationManager IndentationManager { get; } = new();
        ExpandBuffer<int> elementCountStack;
        ExpandBuffer<string> tagStack;
        int currentElementCount;

        public Utf8YamlEmitter(IBufferWriter<byte> writer, YamlEmitOptions? options = null)
        {
            Writer = writer;
            Options = options ?? YamlEmitOptions.Default;

            StateStack = new ExpandBuffer<EmitState>(16);
            elementCountStack = new ExpandBuffer<int>(16);
            StateStack.Add(EmitState.None);
            currentElementCount = 0;

            tagStack = new ExpandBuffer<string>(4);
        }

        public void Dispose()
        {
            StateStack.Dispose();
            elementCountStack.Dispose();
            tagStack.Dispose();
        }

        void BeginBlockMapping()
        {
            switch (StateStack.Current)
            {
                case EmitState.BlockMappingKey:
                    throw new YamlEmitterException("To start block-mapping in the mapping key is not supported.");
                case EmitState.FlowMappingKey:
                    throw new YamlEmitterException("To start flow-mapping in the mapping key is not supported.");
                case EmitState.FlowSequenceEntry:
                    throw new YamlEmitterException("Cannot start block-mapping in the flow-sequence");

                case EmitState.BlockSequenceEntry:
                    {
                        WriteBlockSequenceEntryHeader();
                        break;
                    }
            }
            PushState(EmitState.BlockMappingKey);
        }
        void BeginFlowMapping()
        {
            switch (StateStack.Current)
            {
                case EmitState.BlockMappingKey:
                    throw new YamlEmitterException("To start block-mapping in the flow mapping key is not supported.");
                case EmitState.BlockSequenceEntry:
                    throw new YamlEmitterException("To start block-sequence in the flow mapping key is not supported.");
                case EmitState.FlowSequenceEntry:
                    break;
                case EmitState.FlowMappingKey:
                    {
                        var output = Writer.GetSpan(EmitCodes.FlowMappingStart.Length + 1);
                        var offset = 0;
                        EmitCodes.FlowMappingStart.CopyTo(output);
                        offset += EmitCodes.FlowMappingStart.Length;
                        output[offset++] = YamlCodes.FlowSequenceStart;
                        Writer.Advance(offset);
                        break;
                    }
            }
            PushState(EmitState.FlowMappingKey);
        }
        void BeginBlockSequence()
        {
            switch (StateStack.Current)
            {
                case EmitState.BlockSequenceEntry:
                    WriteBlockSequenceEntryHeader();
                    break;

                case EmitState.FlowSequenceEntry:
                    throw new YamlEmitterException(
                        "To start block-sequence in the flow-sequence is not supported.");
                case EmitState.FlowMappingKey:
                    throw new YamlEmitterException(
                        "To start block-sequence in the flow mapping key is not supported.");
                case EmitState.BlockMappingKey:
                    throw new YamlEmitterException(
                        "To start block-sequence in the mapping key is not supported.");
            }

            PushState(EmitState.BlockSequenceEntry);
        }
        void BeginFlowSequence()
        {
            switch (StateStack.Current)
            {
                case EmitState.BlockMappingKey:
                    throw new YamlEmitterException("To start block-mapping in the mapping key is not supported.");
                case EmitState.FlowMappingKey:
                    throw new YamlEmitterException("To start flow-mapping in the mapping key is not supported.");

                case EmitState.BlockSequenceEntry:
                    throw new YamlEmitterException("To start flow-mapping in the mapping key is not supported.");
                case EmitState.FlowSequenceEntry:
                    {
                        break;
                    }
                case EmitState.BlockMappingValue:
                    break;
                default:
                    throw new Exception(StateStack.Current.ToString());
                    WriteRaw(YamlCodes.FlowSequenceStart);
                    break;
            }
            PushState(EmitState.FlowSequenceEntry);
        }
        public void BeginSequence(DataStyle style = DataStyle.Normal)
        {
            switch (style)
            {
                case DataStyle.Normal:
                    {
                        BeginBlockSequence();
                        break;
                    }
                case DataStyle.Compact:
                    {
                        BeginFlowSequence();
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException(nameof(style), style, null);
            }
        }
        public void EndSequence(bool isEmpty)
        {
            switch (StateStack.Current)
            {
                case EmitState.BlockSequenceEntry:
                    {
                        var isEmptySequence = isEmpty;
                        PopState();

                        // Empty sequence
                        if (isEmptySequence)
                        {
                            var lineBreak = StateStack.Current is EmitState.BlockSequenceEntry or EmitState.BlockMappingValue;
                            WriteRaw(EmitCodes.FlowSequenceEmpty, false, lineBreak);
                        }

                        switch (StateStack.Current)
                        {
                            case EmitState.BlockSequenceEntry:
                                if (!isEmptySequence)
                                {
                                    IndentationManager.DecreaseIndent();
                                }
                                currentElementCount++;
                                break;

                            case EmitState.BlockMappingKey:
                                throw new YamlEmitterException("Complex key is not supported.");

                            case EmitState.BlockMappingValue:
                                StateStack.Current = EmitState.BlockMappingKey;
                                currentElementCount++;
                                break;

                            case EmitState.FlowSequenceEntry:
                                currentElementCount++;
                                break;
                        }
                        break;
                    }

                case EmitState.FlowSequenceEntry:
                    {
                        PopState();

                        var needsLineBreak = false;
                        switch (StateStack.Current)
                        {
                            case EmitState.BlockSequenceEntry:
                                needsLineBreak = true;
                                currentElementCount++;
                                break;
                            case EmitState.BlockMappingValue:
                                StateStack.Current = EmitState.BlockMappingKey; // end mapping value
                                needsLineBreak = true;
                                currentElementCount++;
                                break;
                            case EmitState.FlowSequenceEntry:
                                currentElementCount++;
                                break;
                        }

                        var suffixLength = 1;
                        if (needsLineBreak) suffixLength++;

                        var offset = 0;
                        var output = Writer.GetSpan(suffixLength);
                        output[offset++] = YamlCodes.FlowSequenceEnd;
                        if (needsLineBreak)
                        {
                            output[offset++] = YamlCodes.Lf;
                        }
                        Writer.Advance(offset);
                        break;
                    }

                default:
                    throw new YamlEmitterException($"Current state is not sequence: {StateStack.Current}");
            }
        }
        public void BeginMapping(DataStyle style = DataStyle.Normal)
        {
            if (style is DataStyle.Normal)
                BeginBlockMapping();
            else if (style is DataStyle.Compact)
                BeginFlowMapping();
            else
                throw new ArgumentOutOfRangeException(nameof(style), style, null);
        }
        public void EndMapping()
        {
            if (StateStack.Current != EmitState.BlockMappingKey && StateStack.Current != EmitState.FlowMappingKey)
            {
                throw new YamlEmitterException($"Invalid block mapping end: {StateStack.Current}");
            }
            

            var isEmptyMapping = currentElementCount <= 0;
            if (StateStack.Current == EmitState.FlowMappingKey && !isEmptyMapping)
            {
                WriteRaw(EmitCodes.FlowMappingEnd, false, true);
            } 
            else if(StateStack.Current == EmitState.FlowMappingKey && isEmptyMapping)
            {
                WriteRaw(YamlCodes.FlowMapEnd);
            }
            PopState();

            if (isEmptyMapping)
            {
                var lineBreak = StateStack.Current is EmitState.BlockSequenceEntry or EmitState.BlockMappingValue;
                if (tagStack.TryPop(out var tag))
                {
                    var tagBytes = StringEncoding.Utf8.GetBytes(tag + " "); // TODO:
                    WriteRaw(tagBytes, EmitCodes.FlowMappingEmpty, false, lineBreak);
                }
                else
                {
                    WriteRaw(EmitCodes.FlowMappingEmpty, false, false);
                }
            }

            switch (StateStack.Current)
            {
                case EmitState.BlockSequenceEntry:
                    if (!isEmptyMapping)
                    {
                        IndentationManager.DecreaseIndent();
                    }
                    currentElementCount++;
                    break;

                case EmitState.BlockMappingValue:
                    if (!isEmptyMapping)
                    {
                        IndentationManager.DecreaseIndent();
                    }
                    StateStack.Current = EmitState.BlockMappingKey;
                    currentElementCount++;
                    break;
                case EmitState.FlowMappingValue:
                    if(!isEmptyMapping)
                    {
                        IndentationManager.DecreaseIndent();
                    }
                    StateStack.Current = EmitState.BlockMappingKey;
                    currentElementCount++;
                    break;
                case EmitState.FlowSequenceEntry:
                    currentElementCount++;
                    break;
            }
        }

        public void Tag(string value)
        {
            tagStack.Add(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int CalculateMaxScalarBufferLength(int length)
        {
            var around = (CurrentIndentLevel + 1) * Options.IndentWidth + 3;
            if (tagStack.Length > 0)
            {
                length += StringEncoding.Utf8.GetMaxByteCount(tagStack.Peek().Length) + around; // TODO:
            }
            return length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void PushState(EmitState state)
        {
            StateStack.Add(state);
            elementCountStack.Add(currentElementCount);
            currentElementCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void PopState()
        {
            StateStack.Pop();
            currentElementCount = elementCountStack.Length > 0 ? elementCountStack.Pop() : 0;
        }
    }
}
