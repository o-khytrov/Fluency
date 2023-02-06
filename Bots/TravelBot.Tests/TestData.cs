using Fluency.Engine.Models;
using Fluency.Testing;
using Xunit;

namespace TravelBot.Tests;

public class TestData : TheoryData<TestConversation>
{
    public TestData()
    {
        Add(new TestConversation()
        {
            new()
            {
                UserMessage = new UserMessage() { Text = "I am at Sumy" },
                ExpectedResponse = new BotMessage() { Text = "Ok you are in Sumy" },
            },
            new()
            {
                UserMessage = new UserMessage() { Text = "I want to go to Lviv" },
                ExpectedResponse = new BotMessage() { Text = "Ok you want to go to Lviv" },
            },
            new()
            {
                UserMessage = new UserMessage() { Text = "What do I want" },
                ExpectedResponse = new BotMessage() { Text = "You want to go from Sumy to Lviv" },
            },
            new()
            {
                UserMessage = new UserMessage() { Text = "Thank you" },
                ExpectedResponse = new BotMessage() { Text = "Chat is completed" },
            }
        });
    }
}