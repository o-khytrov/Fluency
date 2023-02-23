using Fluency.Engine;
using Fluency.Engine.Playground;
using Microsoft.Extensions.DependencyInjection;
using SampleBots;
using SampleBots.Harry;
using SampleBots.Travelling;


await new ServiceCollection()
    .AddFluency(x =>
        {
            x.AddBot<TravelAgent>();
            x.AddBot<HarryBot>();
        }
    )
    .AddSingleton<WikipediaClient>()
    .BuildServiceProvider()
    .GetRequiredService<ConsolePlayground>()
    .RunAsync<HarryBot>();