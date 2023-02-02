using FluentConversation.Engine;
using TravelBot.GeneralBot;

var testingConsole = ConsolePlayground.Build();
await testingConsole.RunAsync(new GeneralAssistant());