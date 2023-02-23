namespace Fluency.Engine.PatternSystem.Elements;

public class MultipleWordsPatternElement<T> : PatternElement<T> where T : ChatContext, new()
{
    public List<string> Values { get; set; } = new();

    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    public override bool Match(Conversation<T> conversation, List<string> extracted, Tokenizer tokenizer)
    {
        var input = conversation.CurrentInput;
        if (!input.MoveNext())
        {
            return false;
        }

        var isMatch = false;
        foreach (var value in Values)
        {
            var token = input.Current;
            isMatch = MemoryExtensions.Equals(value, token.ValueAsSpan, StringComparison);
            if (isMatch)
            {
                break;
            }
        }

        return isMatch;
    }
}