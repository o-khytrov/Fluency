// See https://aka.ms/new-console-template for more information

using ConversationDesigner;

Console.WriteLine("What is your name?");
var username = Console.ReadLine();

var bot = new MyBot();
var engine = new ChatEngine();
while (true)
{
    var input = Console.ReadLine();
    var output = engine.Run(bot, input, username);
    Console.WriteLine(output);
}