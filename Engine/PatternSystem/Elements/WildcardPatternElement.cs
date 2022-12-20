using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class WildcardPatternElement : PatternElement
{
    public List<string>? Values { get; set; }

    public override bool Match(TokenCollection input, List<string> extracted)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var token = input.Current;
        if (!string.IsNullOrWhiteSpace(token))
        {
            extracted.Add(token);
            return true;
        }

        return false;
    }
}