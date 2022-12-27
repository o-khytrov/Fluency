using FluentConversation.Engine;

namespace MedicalBot;

public class SmarterMedical : Bot<MedicalBotContext>
{
    public string Sympathy { get; set; } = string.Empty;

    public string QuestionType { get; set; }

    public SmarterMedical()
    {
        R("MEDICAL_START")
            .Output($"Welcome! How can I help you with your {QuestionType} question?");

        R("COST_REJOINDER")
            .Pattern(x =>
            {
                x.Disjunction(d =>
                {
                    d.Add().Word("is", "are", "will").Word("this", "you").Word("free", "paid");
                    d.Add().Word("how", "are", "will").Word("this", "you").Word("free", "paid");
                });
            }).Output("You pay 5$");
    }
}