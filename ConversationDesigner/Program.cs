// See https://aka.ms/new-console-template for more information

using ConversationDesigner;

var bot = new MyBot();
var engine = new ChatEngine();
while (true)
{
    var input = Console.ReadLine();
    var output = engine.Run(bot, input);
    Console.WriteLine(output);
}