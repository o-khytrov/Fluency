using Fluency.Engine.Models;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.Rules;
using Fluency.Engine.Storage;
using Fluency.Engine.Tokenization;

namespace Fluency.Engine;

public class ChatEngine
{
    private readonly IChatContextStorage _chatContextStorage;

    private readonly PatternEngine _patternEngine;
    private readonly Tokenizer _tokenizer;

    public ChatEngine(IChatContextStorage chatContextStorage,
        Tokenizer tokenizer,
        PatternEngine patternEngine
    )

    {
        _chatContextStorage = chatContextStorage;
        _tokenizer = tokenizer;
        _patternEngine = patternEngine;
    }

    /// <summary>
    /// If conversation exists for a given username, erase conversation from the storage if 
    /// </summary>
    /// <param name="username"></param>
    public async Task RestartConversation(string username)
    {
        await _chatContextStorage.DeleteConversation(username);
    }


    public async Task<BotMessage> PerformChatAsync<T>(Bot<T> bot, UserMessage userMessage, string username)
        where T : ChatContext, new()
    {
        var conversation = await _chatContextStorage.GetConversation<T>(username) ?? new Conversation<T>()
            { UserId = username, CurrentTopic = Constants.DefaultTopic };
        conversation.Input++;
        conversation.Messages.Add(userMessage);
        var botInput = new BotInput(userMessage.Text, userMessage.Variables);
        botInput.Document = _tokenizer.Tokenize(botInput.RawInput).ToTokenList();
        var botMessage = bot.Control(_patternEngine, conversation, botInput) ?? new BotMessage
        {
            Text = bot.ChatCompleteMessage
        };

        conversation.Messages.Add(botMessage);
        conversation.Response++;
        await _chatContextStorage.SaveConversation(conversation);
        return botMessage;
    }
}