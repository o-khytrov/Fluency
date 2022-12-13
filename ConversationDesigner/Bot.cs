namespace ConversationDesigner;

public class Bot
{
    public Language Language { get; set; }

    public List<BotRule> BotRules { get; set; }

    public string ChatCompleteMessage { get; set; } = "Chat is completed";
}