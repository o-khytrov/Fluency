using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public abstract class PatternElement
{
    public PatternElementType PatternElementType { get; set; }

    public abstract bool Match(BotInput input, List<string> extracted);
}