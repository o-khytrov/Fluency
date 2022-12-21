using Engine.Tokenization;

namespace Engine;

public class ChatEngine
{
    private readonly IChatContextStorage _chatContextStorage;

    public ChatEngine(IChatContextStorage chatContextStorage)
    {
        _chatContextStorage = chatContextStorage;
    }


    public async Task<BotResponse> Perform(Bot bot, string input, string username)
    {
        var conversation = await _chatContextStorage.GetConversation(username) ?? new Conversation();

        foreach (var rule in bot.BotRules)
        {
            if (conversation.RuleShown.Contains(rule) && !rule.Keep)
            {
                continue;
            }

            var tokenCollection = new BotInput(input);
            var output = rule.Execute(tokenCollection);
            if (!string.IsNullOrWhiteSpace(output))
            {
                conversation.RuleShown.Add(rule);
                return new BotResponse() { Text = output };
            }
        }

        await _chatContextStorage.SaveConversation(conversation);

        return new BotResponse() { Text = bot.ChatCompleteMessage };
    }
}