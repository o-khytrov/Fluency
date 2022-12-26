using FluentConversation.Engine;
using FluentConversation.Engine.Models;
using TravelBot;

Console.WriteLine("What is your name?");
var username = Console.ReadLine() ?? string.Empty;

var bot = new TravelBot.TravelBot();
var engine = new ChatEngine(new InMemoryChatContextStorage());
while (true)
{
    var input = Console.ReadLine() ?? string.Empty;
    var userMessage = new UserMessage() { Text = input };
    var output = await engine.Perform<TravelBotContext>(bot, userMessage, username);
    Console.WriteLine(output.Text);
}