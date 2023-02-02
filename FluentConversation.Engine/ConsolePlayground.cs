using Catalyst;
using FluentConversation.Engine.Models;
using FluentConversation.Engine.PatternSystem;
using Microsoft.Extensions.DependencyInjection;
using Mosaik.Core;
using Newtonsoft.Json;

namespace FluentConversation.Engine;

public class ConsolePlayground
{
    private readonly ChatEngine _chatEngine;

    public ConsolePlayground(ChatEngine chatEngine)
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

    public static ConsolePlayground Build()
    {
        Catalyst.Models.English.Register();
        var serviceProvider = new ServiceCollection()
            .AddSingleton<PatternEngine>()
            .AddSingleton<ChatEngine>()
            .AddSingleton(Pipeline.For(Language.English))
            .AddSingleton<IChatContextStorage, InMemoryChatContextStorage>()
            .AddSingleton<Tokenizer>()
            .AddSingleton<ConsolePlayground>()
            .BuildServiceProvider();

        var consoleTestingHost = serviceProvider.GetRequiredService<ConsolePlayground>();
        return consoleTestingHost;
    }
}