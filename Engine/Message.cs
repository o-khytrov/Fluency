using System.Security.Principal;

namespace Engine;

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
}