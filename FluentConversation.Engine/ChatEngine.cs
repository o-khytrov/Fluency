using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine;

public class ChatEngine
{
    private readonly IChatContextStorage _chatContextStorage;

    public ChatEngine(IChatContextStorage chatContextStorage)
    {
        _chatContextStorage = chatContextStorage;
    }


    public async Task<BotMessage> Perform(Bot bot, UserMessage userMessage, string username)
    {
        var conversation = await _chatContextStorage.GetConversation(username) ?? new Conversation();
        conversation.Messages.Add(userMessage);
        var botMessage = new BotMessage();
        foreach (var rule in bot.BotRules)
        {
            if (conversation.RuleShown.Contains(rule) && !rule.Keep)
            {
                continue;
            }

            var tokenCollection = new BotInput(userMessage.Text, userMessage.Variables);
            var output = rule.Execute(tokenCollection);
            if (!string.IsNullOrWhiteSpace(output))
            {
                conversation.RuleShown.Add(rule);
                botMessage.Text = output;
                conversation.Messages.Add(botMessage);
                await _chatContextStorage.SaveConversation(conversation);
                return botMessage;
            }
        }

        botMessage.Text = bot.ChatCompleteMessage;
        conversation.Messages.Add(botMessage);
        await _chatContextStorage.SaveConversation(conversation);
        await _chatContextStorage.SaveConversation(conversation);
        return botMessage;
    }
}