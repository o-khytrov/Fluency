namespace Engine;

public class InMemoryChatContextStorage : IChatContextStorage
{
    private readonly Dictionary<string, Conversation> _conversations = new();

    public Task<Conversation?> GetConversation(string userId)
    {
        _conversations.TryGetValue(userId, out var conversation);
        return Task.FromResult(conversation);
    }

    public Task SaveConversation(Conversation conversation)
    {
        _conversations[conversation.UserId] = conversation;
        return Task.CompletedTask;
    }
}

public interface IChatContextStorage
{
    public Task<Conversation?> GetConversation(string userId);
    public Task SaveConversation(Conversation conversation);
}