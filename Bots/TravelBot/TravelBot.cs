using System.Diagnostics.CodeAnalysis;
using FluentConversation.Engine;
using Mosaik.Core;

namespace TravelBot;

public class TravelBot : Bot<TravelBotContext>
{
    public Language Language => Language.English;

    [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Byte[]; size: 51MB")]
    [SuppressMessage("ReSharper.DPA", "DPA0003: Excessive memory allocations in LOH", MessageId = "type: System.Byte[]; size: 101MB")]
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
                x.Word("I", "we").Lemma("be").Word("at", "in").Wildcard())
            .Then((x, m) => x.Source = m[0])
            .Output((c) => $"Ok you are in {c.Source}")
            .Keep();

        R("TARGET")
            .Pattern(x =>
                x.Word("I", "we").Lemma("want").Phrase("to go to").Wildcard())
            .Then((x, m) => x.Target = m[0])
            .Output(c => $"Ok you want to go to {c.Target}")
            .Keep();

        R("COST")
            .Pattern(x =>
                x.Or(
                    b => b.Word("cost"),
                    b => b.Word("money"))
            )
            .Output("It costs 5$");

        R("ASK_SOURCE")
            .Keep()
            .Repeat()
            .When((x, i) => x.Source is null)
            .Output("Where are you?");

        R("ASK_DESTINATION")
            .Keep()
            .Repeat()
            .When((x, i) => x.Target is null)
            .Output("Where do you want to go?");

        R("CONFIRMATION")
            .Keep()
            .Repeat()
            .When((x, i) => x.Source is not null && x.Target is not null)
            .Pattern(x => x.Word("What").Word("do").Word("I", "we").Word("want", "desire"))
            .Output((c) => $"You want to go from {c.Source} to {c.Target}");
    }
}