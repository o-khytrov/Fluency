using Fluency.Engine;
using Mosaik.Core;

namespace TravelBot;

public class HarryChatContext : ChatContext
{
    public string Definition { get; set; }
}

public class Harry : Bot<HarryChatContext>
{
    private readonly string[] _mythicalCreatures = new[] { "dragon" };
    private readonly string[] _insects = new[] { "spider" };
    private readonly string[] _animals = new[] { "tiger" };

    private readonly string _ranAway = "I ran away once, but my parents found me and dragged me back. ";

    private readonly string _scared = "I was  scared of machines. I had vivid dreams of the Terminator robots. ";

    public override Language Language => Language.English;

    public override string Name => "Harry";

    private readonly WikipediaClient _wikipediaClient;

    public Harry(WikipediaClient wikipediaClient)
    {
        _wikipediaClient = wikipediaClient;
        Introductions();

        Childhood();

        Topic("keywordless", () =>
        {
            R("WHAT_IS", keep: true)
                .Pattern(x => x.Word("what").Lemma("be").Wildcard())
                .Then((c, m) => { c.Definition = _wikipediaClient.QueryAsync(m[0]).GetAwaiter().GetResult(); })
                .Output(c =>
                {
                    if (string.IsNullOrEmpty(c.Definition))
                    {
                        return "I don't know what it is";
                    }

                    return c.Definition;
                });
        });
    }

    private void Introductions()
    {
        // will match on every return start of a conversation 
        G("HELLO", keep: true)
            .When((x, i) => x.Input == 1)
            .Output(x => $"{OneOf("Welcome back", "Hello again", "Glad you come back", "Hi", "Hi again")}");

        // matches every time on startup of a new conversation
        G("WELCOME", keep: true)
            .When((x, i) => x.Input == 0)
            .Output(x => "Welcome to Fluency");

        G("BEEN_HERE")
            .Output("Have you been here before?")
            .Rejoinder(() =>
            {
                R("BEEN_HERE_YES")
                    .Pattern(x => x.Word("yes"))
                    .Output("Welcome back!");

                R("BEEN_HERE_NO")
                    .Pattern(x => x.Word("no"))
                    .Output("Then welcome to your first time.");
            });

        R("WHAT_IS_YOU_NAME")
            .Pattern(x => x.Phrase("what is your name"))
            .Output("My name is Harry Potter!");
    }

    private void Childhood()
    {
        Topic("childhood", () =>
        {
            G()
                .Output("Did you run away from home when you were little? ")
                .Rejoinder(() =>
                {
                    R()
                        .Pattern(x => x.Word("yes"))
                        .Output("Was it fun");

                    R()
                        .Pattern(x => x.Word("no"))
                        .Output("You should try it some time. Travel broadens the mind.");
                });
            G("RUNAWAY")
                .Output(_ranAway);

            G()
                .Output("What scared you as a kid? ")
                .Rejoinder(() =>
                {
                    R()
                        .Pattern(x => x.Word("dark", "darkness"))
                        .Output("Did you use a nightlite?");

                    R()
                        .Pattern(x => x.Word("food"))
                        .Output("Food? How weird. ");

                    R()
                        .Pattern(x => x.Word("monster", "boogieman"))
                        .Output("There are no non-human monsters out there. ");

                    R()
                        .Pattern(x => x.Word(_mythicalCreatures))
                        .Output("I guess your parents didn't tell you such creatures didn't exist. ");

                    R()
                        .Pattern(x => x.Word(_insects))
                        .Output("Are you afraid of all insects?");

                    R()
                        .Pattern(x => x.Word("snake"))
                        .Output(
                            "You and Indiana Jones. Snakes would rather slither off than attack you. Just don't surprise them.");

                    R()
                        .Pattern(x => x.Word(_animals))
                        .Output("Big animals are scary. Tiny ones are not, except to some girls. ");

                    R()
                        .Pattern(x => x.Word("dream", "nightmare"))
                        .Output("Do you still have bad dreams now?")
                        .Rejoinder(() =>
                        {
                            R()
                                .Pattern(x => x.Word("yes"))
                                .Output("That's too bad. Maybe you should give up sleeping.");

                            R()
                                .Pattern(x => x.Word("no"))
                                .Output("I guess you no longer go to horror movies.");
                        });
                });

            G("SCARE")
                .Output(_scared);

            G()
                .Output("Did you have a happy childhood? ");
        });
        R("HAPPY")
            .Pattern(x => x.Word("how").Wildcard().Word("childhood"))
            .Output(_ranAway);

        R("YOU_AS_A_KID")
            .Pattern(x => x.Word("how").Wildcard().Word("childhood"))
            .Output(_ranAway);

        R()
            .Pattern(x => x.Word("scare", "afraid"))
            .Output(_scared);

        R()
            .Pattern(x => x.Word("you").Word("early", "childhood").Word("memory"))
            .Output(_scared);
    }
}