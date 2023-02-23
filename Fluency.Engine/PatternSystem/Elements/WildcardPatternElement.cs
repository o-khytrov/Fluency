using Catalyst;

namespace Fluency.Engine.PatternSystem.Elements;

public class WildcardPatternElement<T> : PatternElement<T> where T : ChatContext, new()
{
    private readonly Action<IToken>? _action;
    private readonly Action<T, string>? _assign;
    private readonly int _number;

    public WildcardPatternElement(int number = 1, Action<IToken>? action = null, Action<T, string>? assign = null)
    {
        _action = action;
        _number = number;
        _assign = assign;
    }

    public override bool Match(Conversation<T> conversation, List<string> extracted, Tokenizer tokenizer)
    {
        var input = conversation.CurrentInput;
        for (int i = 0; i < _number; i++)
        {
            if (!input.MoveNext())
            {
                return false;
            }

            var token = input.Current;

            extracted.Add(token.Value);

            _action?.Invoke(token);
            _assign?.Invoke(conversation.Context, token.Value);
        }

        return true;
    }
}