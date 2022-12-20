using Engine.Tokenization;

namespace Engine;

public class ChatEngine
{
    public Dictionary<string, Conversation> Conversations { get; set; } = new();

    public string Run(Bot bot, string input, string username)
    {
        if (!Conversations.ContainsKey(username))
        {
            Conversations.Add(username, new Conversation());
        }

        foreach (var rule in bot.BotRules)
        {
            if (Conversations[username].RuleShown.Contains(rule) && !rule.Keep)
            {
                continue;
            }

            var tokenCollection = new TokenCollection(input);
            var output = rule.Execute(tokenCollection);
            if (!string.IsNullOrWhiteSpace(output))
            {
                Conversations[username].RuleShown.Add(rule);
                return output;
            }
        }

        return bot.ChatCompleteMessage;
    }
}