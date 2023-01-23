using FluentConversation.Engine;
using TravelBot;

var host = new ConsoleTestingHost<TravelBot.TravelBot, TravelBotContext>();
await host.Run();