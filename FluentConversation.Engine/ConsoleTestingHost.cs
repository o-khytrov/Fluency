using FluentConversation.Engine.Models;
using Newtonsoft.Json;

namespace FluentConversation.Engine;

public class ConsoleTestingHost
{
    private readonly ChatEngine _chatEngine;

    public ConsoleTestingHost(ChatEngine chatEngine)
    {
        _chatEngine = chatEngine;
    }

    public async Task RunAsync<T>(Bot<T> bot) where T : new()

    {
        Console.WriteLine("Engine created...");
        Console.WriteLine("What is your name?");
        var username = Console.ReadLine() ?? string.Empty;

        while (true)
        {
            var input = Console.ReadLine() ?? string.Empty;
            if (input == ":exit")
            {
                break;
            }

            UserMessage userMessage;
            try
            {
                userMessage = JsonConvert.DeserializeObject<UserMessage>(input);
            }
            catch (Exception)
            {
                userMessage = new UserMessage
                    { Text = input };
            }

            var output = await _chatEngine.PerformChatAsync(bot, userMessage, username);

            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(bot.Name + ": > ");
            Console.WriteLine(output.Text);
            Console.ForegroundColor = color;
        }
    }
}