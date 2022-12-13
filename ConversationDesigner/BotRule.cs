namespace ConversationDesigner;

public class BotRule
{
    private readonly Func<string, bool> _condition;
    private readonly Func<string> _output;

    public BotRule(Func<string, bool> condition, Func<string> output)
    {
        _condition = condition;
        _output = output;
    }

    public string Execute(string input)
    {
        if (_condition(input))
        {
            return _output();
        }

        return string.Empty;
    }
}