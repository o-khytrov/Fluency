using Catalyst;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine.PatternSystem.Elements;

public class WildcardPatternElement<T> : PatternElement<T> where T : ChatContext, new()
{
    private readonly Action<IToken>? _action;
    private readonly int _number;

    public WildcardPatternElement(int number = 1, Action<IToken>? action = null)
    {
        _action = action;
        _number = number;
    }

    public override bool Match(BotInput input, List<string> extracted, Tokenizer tokenizer)
    {
        for (int i = 0; i < _number; i++)
        {
            if (!input.MoveNext())
            {
                return false;
            }

            var token = input.Current;

            extracted.Add(token.Value);

            _action?.Invoke(token);
        }

        return true;
    }
}