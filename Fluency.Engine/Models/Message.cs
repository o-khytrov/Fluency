using System.Text;

namespace Fluency.Engine.Models;

public class Message
{
    public string Text { get; set; } = string.Empty;

    public Dictionary<string, object> Variables { get; set; } = new();
}

public class UserMessage : Message
{
}

public class BotMessage : Message
{
    public string? RuleName { get; set; }

    private readonly StringBuilder _stringBuilder;

    public BotMessage()
    {
        _stringBuilder = new StringBuilder();
    }

    public void AddOutput(string output)
    {
        _stringBuilder.Append(output);
    }
}