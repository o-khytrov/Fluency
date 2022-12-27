using FluentConversation.Engine.Models;
using FluentConversation.Engine.PatternSystem;
using FluentConversation.Engine.Tokenization;

namespace FluentConversation.Engine;

public class ChatEngine
{
    private readonly IChatContextStorage _chatContextStorage;

    private readonly PatternEngine _patternEngine = new();

    public ChatEngine(IChatContextStorage chatContextStorage)
    {
        _chatContextStorage = chatContextStorage;
    }


    public async Task<BotMessage> Perform<T>(Bot<T> bot, UserMessage userMessage, string username) where T : new()
    {
        var conversation = await _chatContextStorage.GetConversation<T>(username) ?? new Conversation<T>() { UserId = username };
        conversation.Messages.Add(userMessage);
        var botMessage = new BotMessage();
        foreach (var rule in bot.BotRules)
        {
            if (conversation.RuleShown.Contains(rule) && !rule.Keep)
            {
                continue;
            }

            var botInput = new BotInput(userMessage.Text, userMessage.Variables);
            if (rule.IsPreConditionTrue(conversation.Context, botInput))
            {
                var isMatch = true;
                if (rule.Pattern is not null)
                {
                    var matchingResult = _patternEngine.Match(rule.Pattern, botInput);
                    isMatch = matchingResult.Match;
                    if (matchingResult.Match)
                    {
                        foreach (var postAction in rule.PostActions)
                        {
                            postAction(conversation.Context, matchingResult);
                        }
                    }
                }

                if (isMatch)
                {
                    conversation.RuleShown.Add(rule);
                    botMessage.RuleName = rule.Name;
                    botMessage.Text = rule.RenderOutput(conversation.Context);
                    conversation.Messages.Add(botMessage);
                    await _chatContextStorage.SaveConversation(conversation);
                    return botMessage;
                }
            }
        }

        botMessage.Text = bot.ChatCompleteMessage;
        conversation.Messages.Add(botMessage);
        await _chatContextStorage.SaveConversation(conversation);
        return botMessage;
    }
}