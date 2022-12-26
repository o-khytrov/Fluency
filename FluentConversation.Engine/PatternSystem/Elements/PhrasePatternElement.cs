using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class PhrasePatternElement : PatternElement
{
    private BotInput _value;

    private readonly StringComparison _stringComparison;

    public PhrasePatternElement(BotInput value, StringComparison? stringComparison = null)
    {
        _value = value;
        _stringComparison = stringComparison ?? StringComparison.OrdinalIgnoreCase;
    }


    public override bool Match(BotInput input, List<string> extracted)
    {
        _value.Reset();

        var isMatch = true;
        while (_value.MoveNext())
        {
            if (!input.MoveNext())
            {
                return false;
            }

            isMatch = string.Equals(_value.Current, input.Current, _stringComparison);
            if (!isMatch)
            {
                break;
            }
        }


        return isMatch;
    }
}