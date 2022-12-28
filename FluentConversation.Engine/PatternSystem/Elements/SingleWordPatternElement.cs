using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class SingleWordPatternElement : PatternElement
{
    public string? Value { get; set; }

    public bool Lemma { get; set; }

    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    public override bool Match(BotInput input, List<string> extracted)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var token = input.Current;
        var isMatch = MemoryExtensions.Equals(Value, Lemma ? token.ValueAsSpan : token.Lemma, StringComparison);

        return isMatch;
    }
}