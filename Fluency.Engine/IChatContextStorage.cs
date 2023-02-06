namespace FluentConversation.Engine;

public class InMemoryChatContextStorage : IChatContextStorage
{
    private readonly Dictionary<string, object> _conversations = new();

    public Task<Conversation<T>?> GetConversation<T>(string userId) where T : new()
    {
        _conversations.TryGetValue(userId, out var conversation);
        return Task.FromResult(conversation as Conversation<T>);
    }

    public Task DeleteConversation(string userId)
    {
        if (_conversations.ContainsKey(userId))
        {
            _conversations.Remove(userId);
        }

        return Task.CompletedTask;
    }

    public Task SaveConversation<T>(Conversation<T> conversation) where T : new()
    {
        _conversations[conversation.UserId] = conversation;
        return Task.CompletedTask;
    }
}

public interface IChatContextStorage
{
    public Task<Conversation<T>?> GetConversation<T>(string userId) where T : new();
    public Task DeleteConversation(string userId);
    public Task SaveConversation<T>(Conversation<T> conversation) where T : new();
}