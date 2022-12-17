namespace ConversationDesigner;

public class BotRule
{
    public Func<string, bool> Condition { get; set; }

    public Func<string> Output { get; set; }

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