using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class ConceptPatternElement : PatternElement
{
    public List<string>? Values { get; set; }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        throw new NotImplementedException();
    }
}