using FluentConversation.Engine;

namespace TravelBot;

public class TravelBot : Bot<TravelBotContext>
{
    public Language Language => Language.English;

    public TravelBot()
    {
        string[] greetings = new[] { "Hello", "Hi", "Hey" };

        Gambit("Hello")
            .WithPattern(x => x.Word(greetings))
            .WithOutput((c) =>
                $"{OneOf("Hello", "Hi", "Hey")}. {OneOf("tell", "say")} me where you are and where do you want to {OneOf("travel", "go")}")
            .Keep()
            .Repeat();

        Gambit("SOURCE")
            .When((x, i) => x.Source is null)
            .WithPattern(x =>
                x.Word("I", "we").Word("am", "are").Word("at", "in").Wildcard())
            .Then((x, m) => x.Source = m[0])
            .WithOutput((c) => $"Ok you are in {c.Source}");

        Gambit("TARGET")
            .When((x, i) => x.Target is null)
            .WithPattern(x =>
                x.Word("I", "we").Word("want").Phrase("to go to").Wildcard())
            .Then((x, m) => x.Target = m[0])
            .WithOutput(c => $"Ok you want to go to {c.Target}");

        Gambit("CONFIRMATION")
            .WithPattern(x => x.Word("What").Word("do").Word("I", "we").Word("want", "desire"))
            .When((x, i) => x.Source is not null && x.Target is not null)
            .WithOutput((c) => $"You want to go from {c.Source} to {c.Target}");
    }
}