namespace Fluency.Engine.PatternSystem.Elements;

public abstract class PatternElement<T>
    where T : ChatContext, new()
{
    public abstract bool Match(Conversation<T> conversation, List<string> extracted, Tokenizer tokenizer);
}