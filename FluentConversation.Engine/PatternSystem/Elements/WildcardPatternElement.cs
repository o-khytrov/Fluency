using Catalyst;
using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class WildcardPatternElement : PatternElement
{
    private readonly Action<IToken>? _action;

    public WildcardPatternElement(Action<IToken>? action = null)
    {
        _action = action;
    }

    public override bool Match(BotInput input, List<string> extracted)
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