using Fluency.Engine.Playground;
using TravelBot;

var testingConsole = ConsolePlayground.Build(username: "Oleksandr");
await testingConsole.RunAsync(new TravelAgent());