using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class SingleWordPatternElement : PatternElement
{
    public string? Value { get; set; }

    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    public override bool Match(BotInput input, List<string> extracted)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var token = input.Current;
        var isMatch = string.Equals(Value, token, StringComparison);

        return isMatch;
    }
}