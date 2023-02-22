using Catalyst;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class SingleWordPatternElement<T> : PatternElement<T> where T : ChatContext, new()
{
    public string? Value { get; set; }

    public bool Lemma { get; set; }

    private readonly Func<IToken, bool>? _predicate;

    public SingleWordPatternElement(Func<IToken, bool>? predicate = null)
    {
        _predicate = predicate;
    }

    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var token = input.Current;
        var isMatch = MemoryExtensions.Equals(Value, Lemma ? token.Lemma : token.ValueAsSpan, StringComparison);
        if (_predicate is not null)
        {
            isMatch = _predicate(token);
        }

        return isMatch;
    }
}