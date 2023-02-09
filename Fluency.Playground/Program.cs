using Fluency.Engine;
using Fluency.Engine.Playground;
using Microsoft.Extensions.DependencyInjection;
using TravelBot;


await new ServiceCollection()
    .AddFluency(x => x.AddBot<TravelAgent>())
    .BuildServiceProvider()
    .GetRequiredService<ConsolePlayground>()
    .RunAsync<TravelAgent, TravelContext>();