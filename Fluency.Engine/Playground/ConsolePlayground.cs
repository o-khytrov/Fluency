using Catalyst;
using FluentConversation.Engine.Models;
using FluentConversation.Engine.PatternSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mosaik.Core;
using Newtonsoft.Json;

namespace FluentConversation.Engine.Playground;

public class ConsolePlayground
{
    private readonly ChatEngine _chatEngine;
    private readonly ConsolePlaygroundOptions _options;

    public ConsolePlayground(ChatEngine chatEngine, IOptions<ConsolePlaygroundOptions> options)
    {
        _chatEngine = chatEngine;
        _options = options.Value;
    }

    public async Task RunAsync<T>(Bot<T> bot) where T : new()

    {
        Console.WriteLine("Engine created...");

        var username = !string.IsNullOrEmpty(_options.Username) ? _options.Username : AskUserName();

        while (true)
        {
            var input = Console.ReadLine() ?? string.Empty;
            if (input == ":exit")
            {
                break;
            }


            if (input == ":restart")
            {
                await _chatEngine.RestartConversation(username);
            }

            UserMessage userMessage;
            try
            {
                userMessage = JsonConvert.DeserializeObject<UserMessage>(input) ?? new UserMessage
                    { Text = input };
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

    private static string AskUserName()
    {
        Console.WriteLine("What is your name?");
        var username = Console.ReadLine() ?? string.Empty;
        return username;
    }

    public static ConsolePlayground Build(string? username = null)
    {
        Catalyst.Models.English.Register();
        var serviceProvider = new ServiceCollection()
            .Configure<ConsolePlaygroundOptions>(x => x.Username = username)
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