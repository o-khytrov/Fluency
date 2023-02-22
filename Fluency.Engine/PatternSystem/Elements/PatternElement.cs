using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public abstract class PatternElement<T>
    where T : ChatContext, new()
{
    public abstract bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer);
}