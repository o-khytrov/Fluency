namespace Fluency.Engine.Storage;

public interface IChatContextStorage
{
    public Task<Conversation<T>?> GetConversation<T>(string userId) where T : ChatContext, new();
    public Task<Conversation?> GetConversation(string userId); 

    public Task SaveConversation<T>(Conversation<T> conversation) where T : ChatContext, new();

    public Task DeleteConversation(string userId);
}