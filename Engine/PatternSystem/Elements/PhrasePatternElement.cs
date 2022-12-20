using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class PhrasePatternElement : PatternElement
{
    private TokenCollection _value;

    private readonly StringComparison _stringComparison;

    public PhrasePatternElement(TokenCollection value, StringComparison? stringComparison = null)
    {
        _value = value;
        _stringComparison = stringComparison ?? StringComparison.OrdinalIgnoreCase;
    }


    public override bool Match(TokenCollection input, List<string> extracted)
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