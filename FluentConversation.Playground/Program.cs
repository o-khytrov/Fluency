using Catalyst;
using FluentConversation.Engine;
using FluentConversation.Engine.PatternSystem;
using Microsoft.Extensions.DependencyInjection;
using Mosaik.Core;
using TravelBot.GeneralBot;


Catalyst.Models.English.Register();
var serviceProvider = new ServiceCollection()
    .AddSingleton<PatternEngine>()
    .AddSingleton<ChatEngine>()
    .AddSingleton(Pipeline.For(Language.English))
    .AddSingleton<IChatContextStorage, InMemoryChatContextStorage>()
    .AddSingleton<Tokenizer>()
    .AddSingleton<ConsoleTestingHost>()
    .BuildServiceProvider();

var bar = serviceProvider.GetRequiredService<ConsoleTestingHost>();
await bar.RunAsync(new GeneralAssistant());