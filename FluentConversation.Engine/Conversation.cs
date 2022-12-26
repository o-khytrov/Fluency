using Engine.Rules;

namespace Engine;

public class Conversation
{
    public List<BotRule> RuleShown { get; set; } = new();

    public List<Message> Messages { get; set; }

    public string UserId { get; set; } = string.Empty;
}