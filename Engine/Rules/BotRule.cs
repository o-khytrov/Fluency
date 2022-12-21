using Engine.PatternSystem;
using Engine.Tokenization;

namespace Engine;

public class BotRule
{
    public List<Func<BotInput, bool>> Conditions { get; set; } = new();

    public Pattern Pattern { get; set; }

    public Func<string> Output { get; set; }

    public List<RuleTest> Tests { get; set; } = new();

    public bool Keep { get; set; }

    public bool Repeat { get; set; }

    public string Name { get; set; }

    public string Execute(BotInput input)
    {
        var isMatch = true;
        foreach (var condition in Conditions)
        {
            if (!condition(input))
            {
                isMatch = false;
                break;
            }
        }

        if (isMatch)
        {
            return Output();
        }

        return string.Empty;
    }
}

public class RuleTest
{
    public string Input { get; set; } = string.Empty;

    public string ExpectedResponse { get; set; } = string.Empty;
}