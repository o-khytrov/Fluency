using Fluency.Engine.PatternSystem.Elements;

namespace Fluency.Engine.PatternSystem;

public class Pattern<T>
    where T : ChatContext, new()
{
    public List<PatternElement<T>> Elements { get; set; } = new();
}