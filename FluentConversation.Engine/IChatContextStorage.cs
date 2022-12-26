namespace FluentConversation.Engine;

public class InMemoryChatContextStorage : IChatContextStorage
{
    private readonly Dictionary<string, object> _conversations = new();

    public Task<Conversation<T>?> GetConversation<T>(string userId) where T : new()
    {
        _conversations.TryGetValue(userId, out var conversation);
        return Task.FromResult(conversation as Conversation<T>);
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
    public Task SaveConversation<T>(Conversation<T> conversation) where T : new();
}