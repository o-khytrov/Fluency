using System.Xml;
using Fluency.Engine;
using Mosaik.Core;

namespace TravelBot.GeneralBot;

public class QuestionProperties
{
    public string QuestionType { get; set; } = "important";

    public string ExpertName { get; set; } = "Expert";
}

public class GeneralContext
{
    public string Category { get; set; } = string.Empty;

    public string Specialty { get; set; } = string.Empty;

    public string Symptom { get; set; } = string.Empty;

    public string QuestionType { get; set; } = "important";

    public string ExpertName { get; set; } = "Expert";
}

public class GeneralAssistant : Bot<GeneralContext>
{
    private readonly Dictionary<string, QuestionProperties> _questionPropertiesMap = new()
    {
        {
            "car", new QuestionProperties
                { QuestionType = "car", ExpertName = "car expert" }
        },
        {
            "law", new QuestionProperties
                { QuestionType = "legal", ExpertName = "lawyer" }
        }
    };

    public override Language Language => Language.English;

    public override string Name => "Pearl";

    public GeneralAssistant()
    {
        R("START")
            .Do((input, context) =>
            {
                context.Category = input.TryGetVariable<string>("category") ?? "general";
                context.Specialty = input.TryGetVariable<string>("specialty") ?? "general";
                if (_questionPropertiesMap.ContainsKey(context.Category))
                {
                    context.QuestionType = _questionPropertiesMap[context.Category].QuestionType;
                    context.ExpertName = _questionPropertiesMap[context.Category].ExpertName;
                }
            })
            .Output(c => $"Hello. How can I help you with your {c.QuestionType} question?. Our {c.ExpertName} can help you");

        R("GLEAN_SYMPTOM_CLOCK")
            .When((x, i) => x.Specialty == "clock_repair")
            .Pattern(x => x.Word(""))
            .Then((x, m) => x.Symptom = "clock");
    }
}