using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class SingleWordPatternElement : PatternElement
{
    public string? Value { get; set; }

    public bool Lemma { get; set; }

    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var token = input.Current;
        var isMatch = MemoryExtensions.Equals(Value, Lemma ? token.Lemma : token.ValueAsSpan, StringComparison);

        return isMatch;
    }
}