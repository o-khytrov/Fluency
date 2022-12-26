using Engine.Tokenization;

namespace Engine.PatternSystem.Elements;

public class WildcardPatternElement : PatternElement
{
    private readonly Action<string>? _action;

    public WildcardPatternElement(Action<string>? action = null)
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
        if (!string.IsNullOrWhiteSpace(token))
        {
            extracted.Add(token);
            _action?.Invoke(token);
            return true;
        }

        return false;
    }
}