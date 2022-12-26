using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine.PatternSystem.Elements;

public class MultipleWordsPatternElement : PatternElement
{
    public List<string> Values { get; set; } = new();

    public StringComparison StringComparison { get; set; } = StringComparison.OrdinalIgnoreCase;

    public override bool Match(BotInput input, List<string> extracted)
    {
        if (!input.MoveNext())
        {
            return false;
        }

        var isMatch = false;
        foreach (var value in Values)
        {
            var token = input.Current;
            isMatch = string.Equals(value, token, StringComparison);
            if (isMatch)
            {
                break;
            }
        }

        return isMatch;
    }
}