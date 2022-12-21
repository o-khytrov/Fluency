namespace Engine;

public class Conversation
{
    public List<BotRule> RuleShown { get; set; } = new();

    public string UserId { get; set; } = string.Empty;
}