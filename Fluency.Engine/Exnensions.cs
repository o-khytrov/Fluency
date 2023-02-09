using Catalyst;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.Playground;
using Microsoft.Extensions.DependencyInjection;
using Mosaik.Core;

namespace Fluency.Engine;

public static class Extensions
{
    public static IServiceCollection AddFluency(this IServiceCollection serviceCollection)
    {
        Catalyst.Models.English.Register();
        serviceCollection
            .Configure<ConsolePlaygroundOptions>(x => x.Username = "user")
            .AddSingleton<PatternEngine>()
            .AddSingleton<ChatEngine>()
            .AddSingleton(Pipeline.For(Language.English))
            .AddSingleton<IChatContextStorage, InMemoryChatContextStorage>()
            .AddSingleton<Tokenizer>()
            .AddSingleton<ConsolePlayground>()
            .BuildServiceProvider();

        return serviceCollection;
    }
}