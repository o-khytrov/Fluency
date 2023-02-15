using Fluency.Engine;
using Mosaik.Core;

namespace TravelBot;

public class TravelAgent : Bot<TravelContext>
{
    public override Language Language => Language.English;

    public override string Name => "Travel agent";

    public TravelAgent()
    {
        AddDefaultTopic();

        AddWeatherTopic();
    }

    private void AddDefaultTopic()
    {
        var greetings = new[] { "Hello", "Hi", "Hey" };

        R("Hello")
            .Pattern(x => x.Word(greetings))
            .Output((c) =>
                $"{OneOf("Hello", "Hi", "Hey")}. tell me where you are and where do you want to {OneOf("travel", "go")}");

        R("SOURCE")
            .Pattern(x =>
                x.Pronoun().Lemma("be").Word("at", "in").Wildcard())
            .Then((context, match) => context.Source = match[1])
            .Output(c => $"Ok you are in {c.Source}");

        R("TARGET")
            .Pattern(x =>
                x.Word("I", "we").Lemma("want").Phrase("to go to").Wildcard())
            .Then((context, match) => context.Target = match[0])
            .Output(c => $"Ok you want to go to {c.Target}");

        R("COST")
            .Pattern(x =>
                x.Or(
                    b => b.Word("cost"),
                    b => b.Word("pay"),
                    b => b.Word("money"),
                    b => b.Word("is", "are", "will").Word("this", "you").Word("free", "paid")
                )
            )
            .Output("It costs 5$");

        R("ASK_SOURCE")
            .When((x, i) => x.Source is null)
            .Output("Where are you?");

        R("ASK_DESTINATION")
            .When((x, i) => x.Target is null)
            .Output("Where do you want to go?");

        R("CONFIRMATION")
            .When((x, i) => x.Source is not null && x.Target is not null)
            .Pattern(x => x.Word("What").Word("do").Word("I", "we").Word("want", "desire"))
            .Output(c => $"You want to go from {c.Source} to {c.Target}")
            .Rejoinder(() =>
            {
                R("THANK")
                    .Pattern(x => x.Word("thank", "thanks"))
                    .Output("you welcome");

                R("WRONG")
                    .Pattern(x => x.Word("wrong"))
                    .Then((c, m) =>
                    {
                        c.Target = null;
                        c.Source = null;
                    })
                    .Output("Sorry. Let's start it from the beginning");
            });

        R("Weather")
            .Pattern(x => x.Word("Weather"))
            .Output("Ok, lets talk about the weather")
            .NexTopic("Weather");
    }

    private void AddWeatherTopic()
    {
        Topic("Weather", () =>
        {
            R("Temperature")
                .Pattern(x => x.Word("temperature"))
                .Output(c => "The temperature in today is 15 degrees");
        });
    }
}