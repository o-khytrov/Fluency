using Catalyst;
using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class PhrasePatternElement : PatternElement
{
    private readonly List<IToken> _value;

    private readonly StringComparison _stringComparison;

    public PhrasePatternElement(List<IToken> value, StringComparison? stringComparison = null)
    {
        _value = value;
        _stringComparison = stringComparison ?? StringComparison.OrdinalIgnoreCase;
    }


    public override bool Match(BotInput input, List<string> extracted)
    {
        var isMatch = true;
        foreach (var value in _value)
        {
            if (!input.MoveNext())
            {
                return false;
            }

            isMatch = MemoryExtensions.Equals(value.ValueAsSpan, input.Current.ValueAsSpan, _stringComparison);
            if (!isMatch)
            {
                break;
            }
        }

        return isMatch;
    }
}