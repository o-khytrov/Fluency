using Catalyst;
using Fluency.Engine.Models;
using Fluency.Engine.PatternSystem;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Mosaik.Core;
using Newtonsoft.Json;

namespace Fluency.Engine.Playground;

public class ConsolePlayground
{
    private readonly ChatEngine _chatEngine;
    private readonly ConsolePlaygroundOptions _options;
    private readonly IServiceProvider _serviceProvider;

    public ConsolePlayground(ChatEngine chatEngine, IOptions<ConsolePlaygroundOptions> options, IServiceProvider serviceProvider)
    {
        _chatEngine = chatEngine;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public async Task RunAsync<TB, TC>() where TB : Bot<TC> where TC : new()

    {
        Console.WriteLine("Engine created...");
        var bot = _serviceProvider.GetRequiredService<TB>();

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
}