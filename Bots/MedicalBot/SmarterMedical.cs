using FluentConversation.Engine;
using Mosaik.Core;

namespace MedicalBot;

public class SmarterMedical : Bot<MedicalBotContext>
{
    public string Sympathy { get; set; } = string.Empty;

    public string QuestionType { get; set; }

    public SmarterMedical()
    {
        R("MEDICAL_START")
            .Output($"Welcome! How can I help you with your {QuestionType} question?");

        R("MEDICAL_THEME_RASH")
            .When((x, i) => x.Theme == Themes.Rash)
            .Output(
                x =>
                    "When did the_rash start? Is it affecting a particular part of body?");

        R("MEDICAL_THEME_RASH")
            .When((x, i) => x.Theme == Themes.Rash)
            .Output(
                x =>
                    "When did the_rash start? Is it affecting a particular part of body?");

        R("COST_REJOINDER")
            .Pattern(x => { x.Or(d => { x.Word("is", "are", "will").Word("this", "you").Word("free", "paid"); }); })
            .Output("You pay 5$");
    }

    public override Language Language => Language.English;

    public override string Name => "Doctor";
}