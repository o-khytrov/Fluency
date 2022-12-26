using FluentConversation.Engine.PatternSystem.Elements;

namespace FluentConversation.Engine.PatternSystem;

public class Pattern
{
    public List<PatternElement> Elements { get; set; } = new();
}