#nullable enable
namespace NexVYaml.Emitter;

public class YamlEmitOptions
{
    public static YamlEmitOptions Default => new();

    public int IndentWidth { get; set; } = 2;
}

