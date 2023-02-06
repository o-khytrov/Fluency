using Fluency.Engine.Playground;
using TravelBot.GeneralBot;

var testingConsole = ConsolePlayground.Build(username: "Oleksandr");
await testingConsole.RunAsync(new GeneralAssistant());