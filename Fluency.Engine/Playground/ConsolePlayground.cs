using Fluency.Engine.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Fluency.Engine.Playground;

public class ConsolePlayground
{
    private readonly ChatEngine _chatEngine;
    private readonly ConsolePlaygroundOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly Stack<string> _history = new();

    public ConsolePlayground(ChatEngine chatEngine, IOptions<ConsolePlaygroundOptions> options,
        IServiceProvider serviceProvider)
    {
        _chatEngine = chatEngine;
        _serviceProvider = serviceProvider;
        _options = options.Value;
    }

    public Task RunAsync<T>() where T : Bot
    {
        var typeOfContext = typeof(T).BaseType?.GenericTypeArguments
                            ?? throw new ArgumentException("Type argument must derive from 'Bot'");
        var internalMethod = GetType().GetMethod(nameof(InternalRunAsync));
        return (Task) internalMethod!
            .MakeGenericMethod(typeof(T), typeOfContext[0])
            .Invoke(this, new object[] { })!;
    }

    public async Task InternalRunAsync<TB, TC>() where TB : Bot<TC> where TC : ChatContext, new()

    {
        Console.WriteLine("Engine created...");
        var bot = _serviceProvider.GetRequiredService<TB>();

        var username = !string.IsNullOrEmpty(_options.Username) ? _options.Username : AskUserName();

        while (true)
        {
            var input = ReadInput();


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

    private string ReadInput()
    {
        var input = Console.ReadLine() ?? string.Empty;
        _history.Push(input);

        return input;
    }

    private static string AskUserName()
    {
        Console.WriteLine("What is your name?");
        var username = Console.ReadLine() ?? string.Empty;
        return username;
    }

    private static void ClearCurrentConsoleLine()
    {
        int currentLineCursor = Console.CursorTop;
        Console.SetCursorPosition(0, Console.CursorTop);
        Console.Write(new string(' ', Console.WindowWidth));
        Console.SetCursorPosition(0, currentLineCursor);
    }
}