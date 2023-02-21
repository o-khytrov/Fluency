namespace Fluency.Engine.Storage;

public class InMemoryChatContextStorage : IChatContextStorage
{
    private readonly Dictionary<string, object> _conversations = new();

    public Task<Conversation<T>?> GetConversation<T>(string userId) where T : ChatContext, new()
    {
        _conversations.TryGetValue(userId, out var conversation);
        return Task.FromResult(conversation as Conversation<T>);
    }

    public Task<Conversation?> GetConversation(string userId)
    {
        _conversations.TryGetValue(userId, out var conversation);
        return Task.FromResult(conversation as Conversation);
    }

    public Task DeleteConversation(string userId)
    {
        if (_conversations.ContainsKey(userId))
        {
            _conversations.Remove(userId);
        }

        return Task.CompletedTask;
    }

    public Task SaveConversation<T>(Conversation<T> conversation) where T : ChatContext, new()
    {
        _conversations[conversation.UserId] = conversation;
        return Task.CompletedTask;
    }
}