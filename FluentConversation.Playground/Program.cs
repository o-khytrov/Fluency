using FluentConversation.Engine;
using FluentConversation.Engine.Models;
using FluentConversation.Engine.PatternSystem;

var engine = new ChatEngine(new InMemoryChatContextStorage(), new Tokenizer(), new PatternEngine());
Console.WriteLine("Engine created...");
Console.WriteLine("What is your name?");
var username = Console.ReadLine() ?? string.Empty;

var bot = new TravelBot.TravelBot();
while (true)
{
    var input = Console.ReadLine() ?? string.Empty;
    var userMessage = new UserMessage() { Text = input };
    var output = await engine.Perform(bot, userMessage, username);
    Console.WriteLine(output.Text);
}