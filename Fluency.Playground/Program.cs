using Fluency.Engine;
using Fluency.Engine.Playground;
using Microsoft.Extensions.DependencyInjection;
using TravelBot;


var services = new ServiceCollection();
services.AddFluency();
services.AddSingleton<TravelAgent>();
var testingConsole = services.BuildServiceProvider().GetRequiredService<ConsolePlayground>();
await testingConsole.RunAsync<TravelAgent, TravelContext>();