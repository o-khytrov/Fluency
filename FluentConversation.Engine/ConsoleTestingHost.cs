using FluentConversation.Engine.Models;
using FluentConversation.Engine.PatternSystem;

namespace FluentConversation.Engine;

public class ConsoleTestingHost<T, R> where T : Bot<R>, new() where R : new()
{
    public async Task Run()

    {
        var tokenizer = new Tokenizer();
        var engine = new ChatEngine(new InMemoryChatContextStorage(), new Tokenizer(), new PatternEngine());
        Console.WriteLine("Engine created...");
        Console.WriteLine("What is your name?");
        var username = Console.ReadLine() ?? string.Empty;

        var bot = new T();
        while (true)
        {
            var input = Console.ReadLine() ?? string.Empty;
            if (input == ":exit")
            {
                break;
            }

            var userMessage = new UserMessage() { Text = input };
            var output = await engine.Perform(bot, userMessage, username);

            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(output.Text);
            Console.ForegroundColor = color;
        }
    }
}