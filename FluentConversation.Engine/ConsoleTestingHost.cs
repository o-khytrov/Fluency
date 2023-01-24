using FluentConversation.Engine.Models;
using FluentConversation.Engine.PatternSystem;

namespace FluentConversation.Engine;

public class ConsoleTestingHost<T, R> where T : Bot<R>, new() where R : new()
{
    private Tokenizer _tokenizer;

    public ConsoleTestingHost()
    {
        _tokenizer = new Tokenizer();
    }

    public async Task Run()

    {
        var engine = new ChatEngine(new InMemoryChatContextStorage(), _tokenizer, new PatternEngine());
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