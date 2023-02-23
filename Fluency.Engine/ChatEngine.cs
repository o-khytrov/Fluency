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
        // Try find the conversation with the user in the storage. 
        // If it is not found create a new one 
        var conversation = await _chatContextStorage.GetConversation<T>(username) ?? new Conversation<T>()
            { UserId = username, CurrentTopic = Constants.DefaultTopic, BotName = bot.GetType().FullName };

        conversation.Input++;
        conversation.Messages.Add(userMessage);

        // Prepare input for bot, tokenization and POS tagging  
        conversation.CurrentInput = PrepareBotInput(userMessage);

        conversation.BeginVolley();

        //Run Control method of the bot and try get response
        //If the bot is not responding return chat complete message
        var botMessage = bot.Control(_patternEngine, conversation) ?? new BotMessage
        {
            Text = bot.ChatCompleteMessage
        };

        //Save the conversation in the storage
        conversation.Messages.Add(botMessage);

        await _chatContextStorage.SaveConversation(conversation);
        return botMessage;
    }

    private BotInput PrepareBotInput(UserMessage userMessage)
    {
        var botInput = new BotInput(userMessage.Text, userMessage.Variables);
        botInput.ProcessedInput = _tokenizer.Tokenize(botInput.RawInput);
        return botInput;
    }
}