using System.Diagnostics.CodeAnalysis;
using Catalyst;
using Fluency.Engine.PatternSystem;
using Fluency.Engine.Playground;
using Fluency.Engine.Storage;
using Microsoft.Extensions.DependencyInjection;
using Mosaik.Core;

namespace Fluency.Engine;

public static class Extensions
{
    [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH",
        MessageId = "type: System.Byte[]; size: 51MB")]
    private static Pipeline BuildPipeline()
    {
        return Pipeline.For(Language.English);
    }

    private static IServiceCollection AddFluency(this IServiceCollection serviceCollection)
    {
        Catalyst.Models.English.Register();
        serviceCollection
            .Configure<ConsolePlaygroundOptions>(x => x.Username = "user")
            .AddSingleton<PatternEngine>()
            .AddSingleton<ChatEngine>()
            .AddSingleton(BuildPipeline())
            .AddSingleton<IChatContextStorage, InMemoryChatContextStorage>()
            .AddSingleton<Tokenizer>()
            .AddSingleton<ConsolePlayground>()
            .BuildServiceProvider();

        return serviceCollection;
    }

    public static IServiceCollection AddFluency(this IServiceCollection serviceCollection,
        Action<IFluencyDependencyBuilder> configure)
    {
        serviceCollection.AddFluency();
        var dependencyBuilder = new FluencyDependencyBuilder(serviceCollection);
        configure.Invoke(dependencyBuilder);
        return serviceCollection;
    }

    public interface IFluencyDependencyBuilder
    {
        void AddBot<T>() where T : Bot;
    }

    private class FluencyDependencyBuilder : IFluencyDependencyBuilder
    {
        private readonly IServiceCollection _serviceCollection;

        public FluencyDependencyBuilder(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }

        public void AddBot<T>() where T : Bot
        {
            _serviceCollection.AddSingleton<T>();
        }
    }
}