using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public abstract class PatternElement
{
    public PatternElementType PatternElementType { get; set; }

    public abstract bool Match(BotInput input, List<string> extracted);
}