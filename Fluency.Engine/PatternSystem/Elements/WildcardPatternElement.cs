using Catalyst;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class WildcardPatternElement : PatternElement
{
    private readonly Action<IToken>? _action;

    public WildcardPatternElement(Action<IToken>? action = null)
    {
        _action = action;
    }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var token = input.Current;

        extracted.Add(token.Value);
        _action?.Invoke(token);
        return true;
    }
}