using System.Threading.Tasks;
using FluentConversation.Engine;
using FluentConversation.Engine.PatternSystem;
using FluentConversation.Testing;
using Xunit;

namespace TravelBot.Tests;

public class UnitTest1
{
    /*
    [Theory]
    [ClassData(typeof(TestData))]
    public async Task Test2(TestConversation conversation)
    {
        await TestBot<TravelBot, TravelBotContext>(conversation);
    }

    private async Task TestBot<TB, TC>(TestConversation testConversation) where TB : Bot<TC>, new() where TC : new()
    {
        var engine = new ChatEngine(new InMemoryChatContextStorage(), new Tokenizer(), new PatternEngine());
        var bot = new TB();
        foreach (var volley in testConversation)
        {
            var output = await engine.PerformChatAsync(bot, volley.UserMessage, testConversation.UserId);

            Assert.Equal(volley.ExpectedResponse.Text, output.Text);
        }
    }
    */
}