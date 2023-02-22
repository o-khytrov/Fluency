using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class ConceptPatternElement<T> : PatternElement<T>
    where T : ChatContext, new()
{
    public List<string>? Values { get; set; }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        throw new NotImplementedException();
    }
}