namespace Engine;

public class BotRule
{
    public Func<string, bool> Condition { get; set; }

    public Func<string> Output { get; set; }
    public List<RuleTest> Tests { get; set; } = new();

    public bool Keep { get; set; }

    public bool Repeat { get; set; }

    public string Name { get; set; }

    public BotRule()
    {
    }

    public BotRule(string name, Func<string, bool> condition, Func<string> output)
    {
        Name = name;
        Condition = condition;
        Output = output;
    }

    public string Execute(string input)
    {
        if (Condition(input))
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