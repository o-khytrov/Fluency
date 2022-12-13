namespace ConversationDesigner;

public class ChatEngine
{
    public string Run(Bot bot, string input)
    {
        foreach (var rule in bot.BotRules)
        {
            var output = rule.Execute(input);
            if (!string.IsNullOrWhiteSpace(output))
            {
                return output;
            }
        }

        return bot.ChatCompleteMessage;
    }
}