using FluentConversation.Engine;

namespace TravelBot;

public class TravelBot : Bot<TravelBotContext>
{
    public Language Language => Language.English;

    public TravelBot()
    {
        var greetings = new[] { "Hello", "Hi", "Hey" };

        R("Hello")
            .Pattern(x => x.Word(greetings))
            .Output((c) =>
                $"{OneOf("Hello", "Hi", "Hey")}. {OneOf("tell", "say")} me where you are and where do you want to {OneOf("travel", "go")}")
            .Keep()
            .Repeat();

        R("SOURCE")
            .Pattern(x =>
                x.Word("I", "we").Word("am", "are").Word("at", "in").Wildcard())
            .Then((x, m) => x.Source = m[0])
            .Output((c) => $"Ok you are in {c.Source}");

        R("TARGET")
            .Pattern(x =>
                x.Word("I", "we").Word("want").Phrase("to go to").Wildcard())
            .Then((x, m) => x.Target = m[0])
            .Output(c => $"Ok you want to go to {c.Target}");

        R("ASK_SOURCE")
            .When((x, i) => x.Source is null)
            .Output("Where are you?");

        R("ASK_DESTINATION")
            .When((x, i) => x.Target is null)
            .Output("Where do you want to go?");

        R("CONFIRMATION")
            .When((x, i) => x.Source is not null && x.Target is not null)
            .Pattern(x => x.Word("What").Word("do").Word("I", "we").Word("want", "desire"))
            .Output((c) => $"You want to go from {c.Source} to {c.Target}");
    }
}