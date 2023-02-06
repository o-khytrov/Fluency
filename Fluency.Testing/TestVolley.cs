using FluentConversation.Engine.Models;

namespace FluentConversation.Testing;

public class TestVolley
{
    public UserMessage UserMessage { get; set; }

    public BotMessage ExpectedResponse { get; set; }

    public BotMessage ActualResponse { get; set; }

    public TimeSpan Time { get; set; }
}